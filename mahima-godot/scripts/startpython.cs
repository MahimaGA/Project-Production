using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

public partial class startpython : MeshInstance3D
{
	public string scriptPath = "res://mahima-opencv/hand_detection.py";
	public string pythonPath = "res://mahima-opencv/hand_detection.exe";

	public Process _pythonProcess;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("python_launcher");

		var scriptOsPath = ProjectSettings.GlobalizePath(scriptPath);
		var pythonOsPath = ProjectSettings.GlobalizePath($"res://{pythonPath}");
		ThreadPool.QueueUserWorkItem(_ => RunPython(scriptOsPath, pythonOsPath));	
	}

	public override void _ExitTree()
    {
        StopPython();
        base._ExitTree();
    }

	public void WriteExitFlag()
    {
        var flagPath = ProjectSettings.GlobalizePath("res://mahima-opencv/exit.flag");
    	File.WriteAllText(flagPath, "");
    	GD.Print($"[startpython.cs] Wrote exit flag → {flagPath}");
    }
	public void RunPython(string assetPath, string pythonExePath)
	{
		var exePath = ProjectSettings.GlobalizePath(pythonPath);
		var exeDir  = Path.GetDirectoryName(exePath);

		GD.Print($"[startpython.cs] launching → {exePath}");

		if (!File.Exists(exePath))
		{
			GD.PrintErr($"Executable not found at {exePath}");
			return;
		}
		try 
		{
			ProcessStartInfo start = new ProcessStartInfo
			{
				FileName         = exePath,
    			WorkingDirectory = exeDir,
				UseShellExecute     = false,
				RedirectStandardError = true,
				CreateNoWindow      = true
			};

			_pythonProcess = new Process
			{
				StartInfo          = start,
				EnableRaisingEvents = true
			};

			_pythonProcess.ErrorDataReceived += (s, e) =>
			{
				if (!string.IsNullOrEmpty(e.Data))
					GD.PrintErr($"PYTHON ERROR: {e.Data}");
			};

			_pythonProcess.Start();
			_pythonProcess.BeginErrorReadLine();
		}
		catch (Exception e)
		{
			GD.PrintRich($"[color=red]LAUNCH ERROR[/color]: {e.Message}");
		}
	}

	public void StopPython()
    {
        if (_pythonProcess != null && !_pythonProcess.HasExited)
    {
        _pythonProcess.WaitForExit(500);

        if (!_pythonProcess.HasExited)
        {
            _pythonProcess.Kill(entireProcessTree: true);
        }
			GD.Print("[startpython] Python process terminated");
		}
    }

}
