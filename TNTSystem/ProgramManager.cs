using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

public class ProgramManager : MonoBehaviour
{    
    static void Main(string[] args)
    {

// #if (DEBUG)
//         Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
//         var pri = (WindowsPrincipal)Thread.CurrentPrincipal;

//         //관리자 권한 이외로 실행했다면 다른 프로세스로 이 프로세스를 실행한다
//         if (!pri.IsInRole(WindowsBuiltInRole.Administrator))
//         {
//             var proc = new ProcessStartInfo()
//             {
//                 WorkingDirectory = Environment.CurrentDirectory,
//                 FileName = Assembly.GetEntryAssembly().Location,
//                 Verb = "RunAs"
//             };

//             if (args.Length >= 1)
//                 proc.Arguments = string.Join(" ", args);

//             //다른 프로세스로 이 프로세스를 실행한다
//             Process.Start(proc);

//             //현재 프로세스 종료
//             return;

//         }
// #endif
    }
}
