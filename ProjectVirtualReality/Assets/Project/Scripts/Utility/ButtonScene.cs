using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;

public class ButtonScene : MonoBehaviour {
	
	[SerializeField] private string level;
	public Text nickname;
	
	public void OnClick()
	{
		PlayerPrefs.SetString("playerName",nickname.text);
		
		
		try {
			nickname.text = Application.dataPath+"/Debug/serverApp.exe";
			Process myProcess = new Process();
			myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			myProcess.StartInfo.CreateNoWindow = true;
			myProcess.StartInfo.UseShellExecute = false;
			myProcess.StartInfo.FileName = Application.dataPath+"/Debug/serverApp.exe";
			
			myProcess.Start();
			Application.LoadLevel(level);
			
			//print(ExitCode);
		} catch (UnityException e){
			print(e);        
		}
		
	}
}
