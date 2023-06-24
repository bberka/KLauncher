namespace KLauncher.Core;

public class KKernel
{
    private static KKernel? Instance;

    private KKernel() {
    }

    public static KKernel This {
        get {
            Instance ??= new KKernel();
            return Instance;
        }
    }
}