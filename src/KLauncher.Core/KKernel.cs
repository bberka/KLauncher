namespace KLauncher.Core;

public class KKernel 
{
	private KKernel() { }
	public static KKernel This {
		get {
			Instance ??= new();
			return Instance;
		}
	}
	private static KKernel? Instance;
}