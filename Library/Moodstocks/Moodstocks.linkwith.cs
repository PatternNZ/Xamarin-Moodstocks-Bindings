using System;
using ObjCRuntime;

[assembly: LinkWith ("Moodstocks.a", LinkTarget.ArmV7 | LinkTarget.ArmV7s | LinkTarget.Simulator, ForceLoad = true)]
