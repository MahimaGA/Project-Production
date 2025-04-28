using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;



public partial class python : MeshInstance3D
{
	public string scriptPath = "D:/Project-Production/mahima-opencv/hand_detection.py";
	public string pythonPath = "D:/Project-Production/mahima-opencv/.venv/Scripts/python.exe";
	public Process _pythonProcess;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var scriptOsPath = ProjectSettings.GlobalizePath(scriptPath);
		// RunPython(assetPath:scriptOsPath);

		ThreadPool.QueueUserWorkItem(_ => RunPython(scriptOsPath));
	}

	private void RunPython(string assetPath)
	{
		try 
		{
			ProcessStartInfo start = new ProcessStartInfo
			{
				FileName            = pythonPath,
				Arguments           = $"{assetPath}",
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
			GD.PrintRich($"LAUNCH ERROR[/color]: {e.Message}");
		}
		
	}

	public void StopPython()
    {
        try
        {
            if (_pythonProcess != null && !_pythonProcess.HasExited)
            {
                _pythonProcess.Kill();
                _pythonProcess.WaitForExit();
                GD.Print("Python process terminated");
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"ERROR STOPPING PYTHON: {e.Message}");
        }
    }

}
