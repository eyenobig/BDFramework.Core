<img src="./BDTemp/Img/logo.png" width = "65%" height = "65%" div align=center />  

[![openupm](https://img.shields.io/npm/v/com.popo.bdframework?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.cn/packages/com.popo.bdframework/)
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg?style=flat-square)](https://github.com/996icu/996.ICU/blob/master/LICENSE)
[![Badge](https://img.shields.io/badge/link-996.icu-%23FF4D5B.svg?style=flat-square)](https://996.icu/#/zh_CN)
[![LICENSE](https://img.shields.io/github/license/yimengfan/BDFramework.Core)](https://github.com/yimengfan/BDFramework.Core/blob/master/LICENSE)

# 作者寄语(Introduction)  
Simple! Easy! Professional!  This‘s a powerful Unity3d game workflow!   
> BDFramework的设计理念永远是：**工业化、流水线化、专业化！**  
> 永远致力于打造高效的商业游戏工作流.
> 
> BDFramework大部分功能开发都是围绕一整条工作流，以**Pipeline**的形式放出.  
> 如:**BuildPipeline、PublishPipeline、DevOps** 等...  
> 对于第三方库使用也都是为了Pipeline深度定制，很多时候为了一些使用体验优化会编写大量的Editor编码.  
> 
> 这也是BDFramework的设计理念之一：**能编辑器解决的，就不要业务层解决！能自动化的，就不要手动!**  
>  BDFramework没有什么看上去很酷炫的功能，大都是一点一滴的积累，一点点的增加自动化，一点点的增加业务编码的体验.  
> 也正是因为有这样的坚持，才会有这套框架的出现.  
> 
> 最后因为一些原因，只能放出一些游戏基础方案Pipeline的实现，  
> 不会有对具体业务逻辑的解决方案，所以整套workflow更像是一套游戏开发脚手架.望理解！     
> 
> 其实BDFramework更想做的事是抛砖引玉!  
>　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　　by 奶泡泡.                        
# 社区(Community)
#### 第九第十艺术交流:763141410 （QQ Group:763141410）  [点击加群](http://shang.qq.com/wpa/qunwpa?idkey=8e33dccb44f8ac09e3d9ef421c8ec66391023ae18987bdfe5071d57e3dc8af3f)
If you find a bug or have some suggestions,please make issue! I'll get back to you!  
任何问题直接提issue,24小时内必解决 （有时候邮件抽风，没收到，需要在群里at下我~）    
github地址: https://github.com/yimengfan/BDFramework.Core  
gitee地址: https://gitee.com/yimengfan/BDFramework.Core (国内比较快)

# 发布（Publish）  
### [Release版本](https://github.com/yimengfan/BDFramework.Core/releases)  
注:所有bug修复和新特性加入会先提交到Master分支。待审核期通过，稳定则会发布Release版本 
### Demo：  
九宫棋（带一套精简的技能系统）:https://gitee.com/yimengfan/TheCatChess  
 ##  V2.1版本:
#### -增加BuildPipeline！  
#### -增加PublishPipeline！  
#### -增加HotfixPipeline！  
#### -全面支持DevOps工作流.
 ##  V2版本:
#### 1.全面升级为UPM管理: [urp版本安装引导]( https://www.yuque.com/naipaopao/eg6gik/xy8dm4)  
#### 2.全面适配URP管线工作流  
#### 3.全面定制Unity Editor环境，升级编辑器操作。更便捷、人性化的开发体验  
#### 4.全面优化框架启动速度,重构部分远古代码。  
#### 5.UFlux UI工作流全面升级：更智能的值绑定、更简单的工作流、更方便的自定义扩展、DI等...
#### 6.更全面的文档  
#### 7.商业级的Demo加入，后续会开放免费商业级项目开发教程   

## V1版本：  

  **C#热更(C# hotfix):**  
  - 自定义编译服务  
  - 可选工程剥离(热更可以不拆工程)  
  - 一键打包热更DLL  
  - 兼容DevOps、CI、CD.   
  
  **表格管理(Table Manage):**   
  - Excel一键生成Class  
  - Excel生成Sqlite、Json、Xml等  
  - SQLite ORM工具(兼容热更)  
  - 自定义表格逻辑检测.  
  - 兼容DevOps、CI、CD.      
 
  **资源管理（Assets Manage）:**  
  - 重新定制目录管理规范  
  - 一套API自动切换AB和Editor模式，保留Resources.load习惯.
  - 可视化连线打包逻辑、0冗余打包.
  - 打包逻辑纠错机制.  
  - 内置增量打包逻辑，防止不同机器、工程打包AB不同情况.
  - 自动图集管理.  
  - 自动搜集Keyword.  
  - 可寻址加载系统.
  - Assetbundle 同、异步加载验证.
  - Assetbundle加载性能检测.
  - Editor下可用.
  - 兼容DevOps、CI、CD.   
  
   **一键版本发布（Publish）:**  
   - 代码、资源、表格一键打包,版本管理自动下载.  
   - 内置本机AB文件服务器  
   - 兼容DevOps、CI、CD.   
   
   **UI工作流(UFlux):**  
  - 提供一套Flux ui管理机制(类似MVI)  
  - 完善的UI管理,可配合任意NGUI、UGUI、FairyGUI等使用  
  - 完整的UI抽象：Windows、Component、State、Props...  
  - 支持UI管理、值绑定、数据监听、数据流、状态管理等  
  - 支持DI依赖注入.  
   
   **业务管理（Logic Manage）:**  
  - 管理器和被管理类自动注册  
  - 在此之上BD实现了ScreenviewManger,UIManager,EventManager...等一些管理器.使用者根据自己的需求可以实现其他的管理器.  
  - Editor下可用.  
   
   **导航机制（Navication）:**  
   - 模块、用户Timeline等导航机制.  
   - 方便做模块调度、划分等逻辑...   
   
   **全面定制Editor:**
   - 提供完整的编辑器生命周期，方便可定制、拓展.
   - **完整的测试用例，保证框架的稳定.**  
   - 所有功能全面兼容DevOps、CI、CD等工具.  
   - 其他大量的定制Editor，以保证使用体验...(太多了统计不过来)  
  
   **有很多细枝末节的系统就不一一列举了...**

## 文档(Document)  
 ### [中文 Wiki](https://www.yuque.com/naipaopao/eg6gik)  
 #### [English Wiki](http://www.nekosang.com)  
 #### [  视频教程（video）](https://www.bilibili.com/video/av78814115/)
 #### [  博客（Blog）](https://zhuanlan.zhihu.com/c_177032018)  
 
# Unity3d支持(Unity3d support )  
#### Unity2018 - [ObsoleteBranch](https://github.com/yimengfan/BDFramework.Core/tree/2018.4.23LTS)  
#### *Unity2019 - Master* （推荐）      
#### Unity2020 - [TestBranch](https://github.com/yimengfan/BDFramework.Core/tree/2020.4LTS)  
#### Unity2021 - [TestBranch](https://github.com/yimengfan/BDFramework.Core/tree/2021.2.13f1)  
# 安装使用(Start)  
#### 推荐Unity版本: 2019.4LTS.

#### OpenUPM(强烈推荐)：  
**​**
教程 https://www.yuque.com/naipaopao/eg6gik/xy8dm4  
#### Release版：
**使用Open UPM更新框架：**  
Step：
- open **Edit/Project Settings/Package Manager**
- add a new Scoped Registry (or edit the existing OpenUPM entry)
   - **Name** package.openupm.cn
   - **URL** https://package.openupm.cn
   - **Scope(s)** com.ourpalm.ilruntime 、com.popo.bdframework
- click Save (or Apply)

Then open the "**Package Manger"** editor windows. 
Switch  menuitem to "**My Registries** ".
You can see the BDFramework ,you can select the new version.


![image.png](https://cdn.nlark.com/yuque/0/2021/png/338267/1639809205952-492144a5-5d1c-4d1b-8a73-e6cc2d7482b7.png#clientId=u119306b4-6d2b-4&crop=0&crop=0&crop=1&crop=1&from=paste&height=226&id=uc5d79ed9&margin=%5Bobject%20Object%5D&name=image.png&originHeight=452&originWidth=402&originalType=binary&ratio=1&rotation=0&showTitle=false&size=31936&status=done&style=none&taskId=udcaf8962-ed23-40e3-9d83-57847c8a37f&title=&width=201)


#### 预览版（紧急修复bug版）:
手动将框架放置在Package目录下
ps:只移动**com.popo.bdframework文件夹**到项目即可
![image.png](https://cdn.nlark.com/yuque/0/2021/png/338267/1632731115669-05c15202-b644-4605-be01-0c779d3ff9ea.png#clientId=u9755688d-e120-4&crop=0&crop=0&crop=1&crop=1&from=paste&height=234&id=u9349f951&margin=%5Bobject%20Object%5D&name=image.png&originHeight=234&originWidth=648&originalType=binary&ratio=1&rotation=0&showTitle=false&size=14315&status=done&style=none&taskId=u0bf01b11-5384-41ff-a1fd-0fcfbc63622&title=&width=648)

## 贡献者名单
[@gaojiexx](https://github.com/gaojiexx)  
[@ricashao](https://github.com/ricashao)  
[@瞎哥](https://github.com/AfarMiss)  
如果需要项目方案定制、企业支持,可以联系 QQ:755737878  
也随时欢迎交流各种技术.    



   

   ## Feature  

   **· TDD workflow, complete test cases:**  
 what? Dare you use a framework (library) without test cases?  
 
   **· DevOps workflow:**  
   This has to wait for a while ~  
      
   **One key export C# hotfix code:**  
In BD, ILRuntime was re-transformed without sub-projects, and a complete script compilation mechanism was written. The packaging tool automatically collected hot code for packaging.
And adapted to commonly used libraries.

 **One key publish:**  
One key publish of codes, resources, and forms, and version management is automatically downloaded
There are many other things that I think are commonly used: such as the event system, what http library, what object pool is too lazy to list

**A complete resource management system, a set of APIs automatically switch between platforms:**  
BD abandoned the Resources directory, and retains the development habits of user Resources.
A set of APIs automatically switch, compatible with AB and Editor modes.

**And there is a relatively complete AssetBundle management mechanism:**   
atlas management, automatic collection Shader, 0 redundant packaging
And bd has made a set of streamlined addressable, no matter your Asset under Streaming or persistent, it can automatically find and load

**Perfect UI workflow(Flux like):**  
There is a complete set of UI workflow in BD (here we only manage the UI logic, not considering ui production), whether you are UGUI NGUI or other.
We provide a set of mechanisms for UI management, value binding, data monitoring, data flow, state management, etc.  
   
   **Perfect UI workflow:**  
   There is a complete set of UI workflow in BD (here we only manage the UI logic, not considering ui production), whether you are UGUI NGUI or other.  
   We provide a set of mechanisms for UI management, value binding, data monitoring, and data flow.  
   We expect to complete the further upgrade of the UI system in Q4 2018, hoping to create a more advanced and scientific workflow.    
   
   **SQL table management:**  
Sqlite is used to manage forms in BD, and excel2code, excel2json, excel2sqlite and other tools are provided

   **Discovery business registration:**  
The bottom layer of BDFrame provides a set of discovery-type business registration. Without the previous various Registers, as long as you customize your own labels and managers, you can be automatically registered.
On top of this, BD implements a series of manager such as ScreenviewManger, UIManager, EventManager...etc.
This mechanism is highly extensible and customizable, and users can implement other managers according to their own needs
And this is also effective in the editor environment~ It will be very helpful when writing tools~

   **Module management and scheduling:**  
BD brings you a development idea, the user uses the timeline of the process (not the timeline of unity),
Divide the module and schedule according to the user process.The module here is not a narrow window~
     
   </br>There are many other things that I think are commonly used: such as the event system, what http library, what object pool is too lazy to list  

