using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

//디자이너 : 김주하
//웹 프론트 : 김남훈
//웹 백엔드 : 김남훈
//DB 설계 : 김남훈
//Mybatis 설계 : 김남훈
//Logging 설정 : 김남훈
//Layout 구성 : 김남훈
//Model, Controller, View 구성 : 김남훈
//Service, Dao 정의 : 김남훈
[assembly: AssemblyTitle("VisitorSystem")]
[assembly: AssemblyDescription("내방객 시스템. 201809 by 김남훈")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Nhkim")]
[assembly: AssemblyCopyright("Copyright © Microsoft 2018 And 김남훈")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("ko-kr")]

[assembly: ComVisible(false)]

[assembly: Guid("aa8178f5-d968-4908-ae56-7235477f1139")]

[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]

// Log4net Assembly 컨피그 파일 영역
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]