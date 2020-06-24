#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Text.RegularExpressions;


public class iVidCapPro_PostBuild
{

	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		UnityEngine.Debug.Log( "iVidCapPro_PostBuild processing starts... Unity Version = "
			+ Application.unityVersion );
		
		string appControllerName = "UnityAppController.mm";

		string appControllerPath = pathToBuiltProject + Path.DirectorySeparatorChar + "Classes" +
		                           Path.DirectorySeparatorChar + appControllerName;
		string appControllerBackupPath = pathToBuiltProject + Path.DirectorySeparatorChar + "Classes" +
		                                 Path.DirectorySeparatorChar + appControllerName + "_pre_ivcp_edit";

		// The regular expression pattern to be used for finding the place in AppController
		// that we want to insert our get context function.
		string insertionPointPattern = "// --- AppController";

		string contextFunction_4 = 
			"\n" +
			"// Added for use by iVidCapPro.\n" +
			"extern \"C\" EAGLContext* ivcp_UnityGetContext()\n" +
			"{\n" +
			"	return UnityGetMainScreenContextGLES();\n" +
			"}\n\n";

		// Make a backup copy of the AppController.
		File.Copy( appControllerPath, appControllerBackupPath, true );
		if ( !File.Exists( appControllerBackupPath ) )
		{
			UnityEngine.Debug.LogError( "iVidCapPro_PostBuild ERROR: Backup of the original " + appControllerName + " could not be created." );
			return;
		}
		else
		{
			UnityEngine.Debug.Log( "iVidCapPro_PostBuild: Backup of the original " + appControllerName + " file created as: " +
				Path.GetFileName( appControllerBackupPath ) );
		}

		// Read App Controller file into a string.
		string fileString = "";
		if ( !ReadFileIntoString( appControllerPath, out fileString ) )
		{
			// Read failed...
			UnityEngine.Debug.LogError( "iVidCapPro_PostBuild ERROR: Could not read file " + appControllerName + "." );
			return;
		}

		string substString = "";
		substString = contextFunction_4;

		// Check to see if AppController already contains our function.
		if ( !Regex.IsMatch( fileString, @"ivcp_UnityGetContext" ) )
		{
			// For 4.5 and later, just add our function at the end of the file.
			fileString += substString;
			// Write modified AppController back to file.
			WriteStringIntoFile( fileString, appControllerPath );
		}
		else
		{
			// The function is already present.  No action needed.
			UnityEngine.Debug.Log( "iVidCapPro_PostBuild: ivcp_UnityGetContext function already present. Nothing done." ); 
		}

		UnityEngine.Debug.Log( "iVidCapPro_PostBuild processing completed successfully." );

		return;
	}

	private static bool ReadFileIntoString(string filePath, out string stringData)
	{

		bool result = true;
		stringData = "";
		if ( File.Exists( filePath ) )
		{
			stringData = File.ReadAllText( filePath );
		}
		else
		{
			result = false;
		}
		return result;
	}

	private static bool WriteStringIntoFile(string stringData, string filePath)
	{

		File.WriteAllText( filePath, stringData );
		return true;
	}

}
#endif