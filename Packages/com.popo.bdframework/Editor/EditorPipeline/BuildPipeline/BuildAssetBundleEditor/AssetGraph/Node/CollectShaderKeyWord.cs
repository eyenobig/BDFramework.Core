﻿ using System;
using System.Collections.Generic;
using System.Linq;
using BDFramework.Editor.AssetBundle;
 using BDFramework.Editor.BuildPipeline.AssetBundle;
 using BDFramework.ResourceMgr;
using UnityEditor;
using UnityEngine;
using UnityEngine.AssetGraph;
using UnityEngine.AssetGraph.DataModel.Version2;

namespace BDFramework.Editor.AssetGraph.Node
{
    [CustomNode("BDFramework/[逻辑]搜集shader变体", 60)]
    public class CollectShaderKeyWord : UnityEngine.AssetGraph.Node
    {
        public AssetBundleBuildingContext BuildingCtx { get; set; }

        public void Reset()
        {
            this.isCollectedShaderKW = false;
        }

        public override string ActiveStyle
        {
            get { return "node 7 on"; }
        }

        public override string InactiveStyle
        {
            get { return "node 7"; }
        }

        public override string Category
        {
            get { return "搜集Shader变体"; }
        }

        public override void Initialize(NodeData data)
        {
            data.AddDefaultInputPoint();
            data.AddDefaultOutputPoint();
        }

        public override UnityEngine.AssetGraph.Node Clone(NodeData newData)
        {
            return new CollectShaderKeyWord();
        }

        private NodeGUI selfNodeGUI;
        public override void OnDrawNodeGUIContent(NodeGUI node)
        {
            base.OnDrawNodeGUIContent(node);
            this.selfNodeGUI = node;
        }

        public override void OnInspectorGUI(NodeGUI node, AssetReferenceStreamManager streamManager, NodeGUIInspector inspector, Action onValueChanged)
        {
        }

        //后缀和ab名
        private List<string> FileExtens = new List<string>() {".shader", ".shadervariants"};
        private string AssetBundleName = BResources.ALL_SHADER_VARAINT_ASSET_PATH;

        private bool isCollectedShaderKW = false;

        /// <summary>
        /// 预览结果 编辑器连线数据，但是build模式也会执行
        /// 这里只建议设置BuildingCtx的ab颗粒度
        /// </summary>
        /// <param name="target"></param>
        /// <param name="nodeData"></param>
        /// <param name="incoming"></param>
        /// <param name="connectionsToOutput"></param>
        /// <param name="outputFunc"></param>
        public override void Prepare(BuildTarget target, NodeData nodeData, IEnumerable<PerformGraph.AssetGroups> incoming, IEnumerable<ConnectionData> connectionsToOutput, PerformGraph.Output outputFunc)
        {
            if (incoming == null)
            {
                return;
            }
            //搜集所有的 asset reference 
            var comingAssetReferenceList = AssetGraphTools.GetComingAssets(incoming);
            if (comingAssetReferenceList.Count == 0)
            {
                return;
            }
            this.BuildingCtx = BDFrameworkAssetsEnv.BuildingCtx;
            
            
            
            AssetGraphTools.WatchBegin();
            //收集变体
            if (!isCollectedShaderKW || this. BuildingCtx.BuildParams.IsBuilding) //防止GUI每次调用prepare时候都触发,真正打包时候 会重新构建
            {
                Debug.Log("------------>收集Key word");
                var allAssets = comingAssetReferenceList.Select((a) => a.importFrom).ToArray();
                ShaderCollection.CollectShaderVariant(allAssets);
                isCollectedShaderKW = true;
            }

            //开始搜集传入的 shader varint
            var outMap = new Dictionary<string, List<AssetReference>>();
            var incomingShaderAndVariantList = new List<AssetReference>();
            var dependice = AssetDatabase.GetDependencies(BResources.ALL_SHADER_VARAINT_ASSET_PATH);
            
            var dependShaders = dependice.Where((depend) =>
            {
                var type = AssetDatabase.GetMainAssetTypeAtPath(depend);
                if (type == typeof(Shader) || type == typeof(ShaderVariantCollection))
                {
                    //Debug.LogError("【搜集Shader】剔除非shader文件" + type.FullName);
                    return true;
                }
                return false;
            });

            //遍历传入的并且移除shader需要的
            foreach (var ags in incoming)
            {
                foreach (var group in ags.assetGroups)
                {
                    //拷贝一份
                    var newList = group.Value.ToList();
                    //
                    for (int i = newList.Count - 1; i >= 0; i--)
                    {
                        //不直接操作传入的容器存储
                        var af = newList[i];
                        var ret = dependShaders.FirstOrDefault((dp) => dp.Equals(af.importFrom, StringComparison.OrdinalIgnoreCase));
                        if (ret != null)
                        {
                            newList.RemoveAt(i);
                            incomingShaderAndVariantList.Add(af);
                        }
                        else
                        {
                            if (af.assetType == typeof(Shader))
                            {
                                Debug.LogError($"【搜集KeyWord】 遗漏: {af.importFrom},\n 1.请检查是否直接引用了FBX这类,SubAsset中有Mat的资产,如是请Ctrl+D复制引用! \n 2.注意shader的依赖shader情况" );
                                
                                //寻找遗漏查找依赖资源
                              var shaderAssetData = this.BuildingCtx.BuildAssetInfos.GetAssetInfo(af.importFrom );
                                if (shaderAssetData != null)
                                {
                                    var returnBD = this.BuildingCtx.BuildAssetInfos.AssetInfoMap.FirstOrDefault((bd) => bd.Value.DependAssetList.Contains(shaderAssetData.ABName) || bd.Value.DependAssetList.Contains(af.importFrom, StringComparer.Ordinal));
                                    if (returnBD.Value != null)
                                    {
                                        Debug.LogError("主资源:" + returnBD.Key);
                                    }
                                }
                            }
                        }
                    }

                    //输出
                    outMap[group.Key] = newList;
                }
            }

            //依赖shader
            if (incomingShaderAndVariantList.Count > 0) //0的情况一般为 调试模式~
            {
                foreach (var ds in dependShaders)
                {
                    foreach (var incom in incomingShaderAndVariantList)
                    {
                        if (incom.importFrom.Equals(ds, StringComparison.OrdinalIgnoreCase))
                        {
                          
                        }
                        
                    }
                    
                    var retsult = incomingShaderAndVariantList.FirstOrDefault((ar) => ar.importFrom.Equals(ds, StringComparison.OrdinalIgnoreCase));
                    if (retsult == null)
                    {
                        var af = AssetReference.CreateReference(ds);
                        incomingShaderAndVariantList.Add(af);
                        Debug.LogError("没传入的依赖shader 单独添加：" + ds);
                    }
                }

            }
            
            //设置ab
            foreach (var sharder in incomingShaderAndVariantList)
            {
                var (ret,msg) = this.BuildingCtx.BuildAssetInfos.SetABPack(sharder.importFrom, AssetBundleName, BuildAssetInfos.SetABPackLevel.Force,this.Category+"  "+(this.selfNodeGUI!=null?this.selfNodeGUI.Name: this.GetHashCode().ToString()),false);
                // if (!ret)
                // {
                //     Debug.LogError(msg);
                // }
            }

            AssetGraphTools.WatchEnd("【搜集KeyWord】");
            //输出shader
            outMap[nameof(BDFrameworkAssetsEnv.FloderType.Shaders)] = incomingShaderAndVariantList;
            //输出
            var output = connectionsToOutput?.FirstOrDefault();
            if (output != null)
            {
                outputFunc(output, outMap);
            }
        }


        public override void Build(BuildTarget target, NodeData nodeData, IEnumerable<PerformGraph.AssetGroups> incoming, IEnumerable<ConnectionData> connectionsToOutput, PerformGraph.Output outputFunc, Action<NodeData, string, float> progressFunc)
        {
            //简单生成ShaderVarrint
        }
    }
}