using Godot;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;



public partial class python : MeshInstance3D
{
	public string scriptPath = "D:/Project-Production/mahima-opencv/hand_detection.py";
	public string pythonPath = "D:/Project-Production/mahima-opencv/.venv/Scripts/python.exe";
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
		
			using Process process = Process.Start(start);

			string error = process.StandardError.ReadToEnd();
            //process.WaitForExit();

			if (!string.IsNullOrEmpty(error))
				GD.PrintRich($"PYTHON ERROR[/color]: {error}");
		}
		catch (Exception e)
		{
			GD.PrintRich($"LAUNCH ERROR[/color]: {e.Message}");
		}
		
	}

}
