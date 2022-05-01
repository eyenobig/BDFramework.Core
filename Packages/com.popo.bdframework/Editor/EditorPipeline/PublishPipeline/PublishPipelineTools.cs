using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BDFramework.Core.Tools;
using BDFramework.Editor.AssetBundle;
using BDFramework.Editor.AssetGraph.Node;
using BDFramework.ResourceMgr;
using BDFramework.ResourceMgr.V2;
using BDFramework.VersionController;
using DotNetExtension;
using LitJson;
using ServiceStack.Text;
using UnityEditor;
using UnityEngine;


namespace BDFramework.Editor
{
    static public class PublishPipelineTools
    {
        static public string UPLOAD_FOLDER_SUFFIX = "_ReadyToUpload";

        /// <summary>
        /// 资源转hash
        /// </summary>
        /// <param name="path"></param>
        /// <param name="uploadHttpApi"></param>
        static public void PublishAssetsToServer(string path)
        {
            var plarforms = new RuntimePlatform[] {RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};


            foreach (var platform in plarforms)
            {
                //资源路径
                var sourcePath = IPath.Combine(path, BDApplication.GetPlatformPath(platform));
                //输出路径
                var outputPath = IPath.Combine(path.Replace("\\", "/"), UPLOAD_FOLDER_SUFFIX, BDApplication.GetPlatformPath(platform));
                if (Directory.Exists(sourcePath))
                {
                    //对比Assets.info 是否一致
                    var sourceAssetsInfoPath = IPath.Combine(sourcePath, BResources.ASSETS_INFO_PATH);
                    var outputAssetsInfoPath = IPath.Combine(outputPath, BResources.ASSETS_INFO_PATH);
                    if (File.Exists(sourceAssetsInfoPath) && File.Exists(outputAssetsInfoPath))
                    {
                        var sourceHash = FileHelper.GetMurmurHash3(sourceAssetsInfoPath);
                        var outputHash = FileHelper.GetMurmurHash3(outputAssetsInfoPath);
                        if (sourceHash == outputHash)
                        {
                            Debug.Log("【PublishPipeline】资源无改动，无需重新生成服务器文件.  -" + BDApplication.GetPlatformPath(platform));
                            continue;
                        }
                    }
                    
                    //获取上一次版本
                    string lastVersionNum = "0.0.0";
                    var serverVersionInfoPath =  IPath.Combine(outputPath,BResources.SERVER_ASSETS_VERSION_INFO_PATH);
                    if (File.Exists(serverVersionInfoPath))
                    {
                        var content = File.ReadAllText(serverVersionInfoPath);
                        var info = JsonMapper.ToObject<AssetsVersionInfo>(content);
                        lastVersionNum = info.Version;
                    }

                    string newVersionNum = "0.0.1";
                    //发布资源处理前,处理前回调
                    BDFrameworkPublishPipelineHelper.OnBeginPublishAssets(platform, sourcePath, lastVersionNum, out newVersionNum);
                    //处理资源
                    var outdir = GenServerHashAssets(path, platform, newVersionNum);
                    //发布资源处理后,通知回调
                    BDFrameworkPublishPipelineHelper.OnEndPublishAssets(platform, outdir);
                    Debug.Log("发布资源处理完成! 请继承PublishPipeline生命周期,完成后续自动化部署到自己的文件服务器!");
                }
            }

          
        }


        /// <summary>
        /// 获取资源hash数据
        /// </summary>
        /// <param name="assetsRootPath"></param>
        /// <returns></returns>
        static public List<ServerAssetItem> GetAssetsHashData(string assetsRootPath, RuntimePlatform platform)
        {
            Debug.Log($"<color=red>------>生成服务器配置:{platform}</color>");
            //黑名单
            List<string> blackFileList = new List<string>()
            {
                BResources.EDITOR_ASSET_BUILD_INFO_PATH,
                BResources.ASSETS_INFO_PATH,
                BResources.ASSETS_SUB_PACKAGE_CONFIG_PATH,
                string.Format("{0}/{0}", BResources.ASSET_ROOT_PATH),
            };
            //混淆文件添加黑名单
            blackFileList.AddRange(AssetBundleBuildingContext.GetMixAssets());

            //加载assetbundle配置
            assetsRootPath = string.Format("{0}/{1}", assetsRootPath, BDApplication.GetPlatformPath(platform));
            var abConfigPath = string.Format("{0}/{1}", assetsRootPath, BResources.ASSET_CONFIG_PATH);
            var abConfigLoader = new AssetbundleConfigLoder();
            abConfigLoader.Load(abConfigPath, null);
            //生成hash配置
            var assets = Directory.GetFiles(assetsRootPath, "*", SearchOption.AllDirectories);
            float count = 0;
            int notABCounter = 1000000;
            //开始生成hash
            var serverAssetItemList = new List<ServerAssetItem>();
            foreach (var assetPath in assets)
            {
                count++;
                EditorUtility.DisplayProgressBar(" 获取资源hash", string.Format("生成文件hash:{0}/{1}", count, assets.Length), count / assets.Length);
                var ext = Path.GetExtension(assetPath).ToLower();
                // bool isConfigFile = false;
                //无效数据
                if (ext == ".manifest" || ext == ".meta")
                {
                    continue;
                }

                //本地的相对路径 
                var localPath = assetPath.Replace("\\", "/").Replace(assetsRootPath + "/", "");

                //黑名单
                var ret = blackFileList.FirstOrDefault((bf) => localPath.Equals(bf, StringComparison.OrdinalIgnoreCase) || Path.GetFileName(localPath).Equals(bf));
                if (ret != null)
                {
                    Debug.Log("【黑名单】剔除:" + ret);
                    continue;
                }

                //文件信息
                var fileHash = FileHelper.GetMurmurHash3(assetPath);
                //  var fileHash2 = FileHelper.GetMurmurHash2(assetPath);
                var fileInfo = new FileInfo(assetPath);
                //
                var abpath = Path.GetFileName(assetPath);
                var assetbundleItem = abConfigLoader.AssetbundleItemList.Find((ab) => ab.AssetBundlePath != null && ab.AssetBundlePath == abpath);
                ServerAssetItem item;
                //文件容量
                float fileSize = (int) ((fileInfo.Length / 1024f) * 100f) / 100f;
                //用ab资源id添加
                if (assetbundleItem != null)
                {
                    item = new ServerAssetItem() {Id = assetbundleItem.Id, HashName = fileHash, LocalPath = localPath, FileSize = fileSize};
                }
                else
                {
                    notABCounter++;
                    item = new ServerAssetItem() {Id = notABCounter, HashName = fileHash, LocalPath = localPath, FileSize = fileSize};
                }

                serverAssetItemList.Add(item);
            }

            EditorUtility.ClearProgressBar();
            //按id排序
            serverAssetItemList.Sort((a, b) =>
            {
                if (a.Id < b.Id)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });

            return serverAssetItemList;
        }

        /// <summary>
        /// 文件转hash
        /// </summary>
        /// <param name="otputPath"></param>
        /// <param name="platform"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        static public string GenServerHashAssets(string otputPath, RuntimePlatform platform, string version)
        {
            Debug.Log($"<color=red>------>生成服务器Hash文件:{BDApplication.GetPlatformPath(platform)}</color>");
            //文件夹准备
            var outputDir = IPath.Combine(otputPath.Replace("\\", "/"), UPLOAD_FOLDER_SUFFIX, BDApplication.GetPlatformPath(platform));
            if (Directory.Exists(outputDir))
            {
                Directory.Delete(outputDir, true);
            }

            Directory.CreateDirectory(outputDir);

            //获取资源的hash数据
            var allServerAssetItemList = GetAssetsHashData(otputPath, platform);
            foreach (var assetItem in allServerAssetItemList)
            {
                var localpath = IPath.Combine( otputPath, BDApplication.GetPlatformPath(platform), assetItem.LocalPath);
                var copytoPath =  IPath.Combine(  outputDir, assetItem.HashName);
                File.Copy(localpath, copytoPath);
            }

            //服务器版本信息
            var serverAssetsInfo = new AssetsVersionInfo();
            
            //生成分包信息
            //加载assetbundle配置
            // var abConfigPath = string.Format("{0}/{1}", assetsRootPath, BResources.ASSET_CONFIG_PATH);
            // var abConfigLoader = new AssetbundleConfigLoder();
            // abConfigLoader.Load(abConfigPath, null);
            var path =  IPath.Combine( otputPath, BDApplication.GetPlatformPath(platform), BResources.ASSETS_SUB_PACKAGE_CONFIG_PATH);
            if (File.Exists(path))
            {
                var subpackageList = CsvSerializer.DeserializeFromString<List<SubPackageConfigItem>>(File.ReadAllText(path));
                foreach (var subPackageConfigItem in subpackageList)
                {
                    var subPackageItemList = new List<ServerAssetItem>();
                    //美术资产
                    foreach (var id in subPackageConfigItem.ArtAssetsIdList)
                    {
                        //var assetbundleItem = abConfigLoader.AssetbundleItemList[id];
                        var serverAssetsItem = allServerAssetItemList.Find((item) => item.Id == id);
                        subPackageItemList.Add(serverAssetsItem);

                        if (serverAssetsItem == null)
                        {
                            Debug.LogError("不存在art asset:" + id);
                        }
                    }

                    //脚本
                    foreach (var hcName in subPackageConfigItem.HotfixCodePathList)
                    {
                        var serverAssetsItem = allServerAssetItemList.Find((item) => item.LocalPath == hcName);
                        subPackageItemList.Add(serverAssetsItem);
                        if (serverAssetsItem == null)
                        {
                            Debug.LogError("不存在code asset:" + hcName);
                        }
                    }

                    //表格
                    foreach (var tpName in subPackageConfigItem.TablePathList)
                    {
                        var serverAssetsItem = allServerAssetItemList.Find((item) => item.LocalPath == tpName);
                        subPackageItemList.Add(serverAssetsItem);

                        if (serverAssetsItem == null)
                        {
                            Debug.LogError("不存在table asset:" + tpName);
                        }
                    }

                    //配置
                    foreach (var confName in subPackageConfigItem.ConfAndInfoList)
                    {
                        var serverAssetsItem = allServerAssetItemList.Find((item) => item.LocalPath == confName);
                        subPackageItemList.Add(serverAssetsItem);
                        if (serverAssetsItem == null)
                        {
                            Debug.LogError("不存在conf:" + confName);
                        }
                    }

                    //
                    subPackageItemList.Sort((a, b) =>
                    {
                        if (a.Id < b.Id)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    });
                    //写入本地配置
                    var subPackageName = string.Format(BResources.SERVER_ART_ASSETS_SUB_PACKAGE_INFO_PATH, subPackageConfigItem.PackageName);
                    var subPackageInfoPath = IPath.Combine(outputDir, subPackageName);
                    var configContent = CsvSerializer.SerializeToString(subPackageItemList);
                    File.WriteAllText(subPackageInfoPath, configContent);
                    Debug.Log("生成分包文件:" + Path.GetFileName(subPackageInfoPath));
                    //写入subPck - version
                    serverAssetsInfo.SubPckMap[subPackageName] = version;

                }
            }
            //生成服务器AssetInfo
            var csv = CsvSerializer.SerializeToString(allServerAssetItemList);
            var configPath = IPath.Combine(outputDir, BResources.ASSETS_INFO_PATH);
            File.WriteAllText(configPath, csv);

            //生成服务器版本号
            serverAssetsInfo.Platfrom = BDApplication.GetPlatformPath(platform);
            serverAssetsInfo.Version = version;
            var json = JsonMapper.ToJson(serverAssetsInfo);
             configPath = IPath.Combine(outputDir, BResources.SERVER_ASSETS_VERSION_INFO_PATH);
            File.WriteAllText(configPath, json);

            return outputDir;
        }
    }
}
