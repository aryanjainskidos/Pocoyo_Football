using UnityEngine;

namespace Zinkia {

    public class Utils {
        
        public enum TypeDevice
        {
            TD_Tablet = 0,
            TD_Smartphone = 1
        }

        public static TypeDevice getTypeDevice()
        {
            float dpi = Screen.dpi;
            float refDpi = 160.0f;
            float density = dpi / refDpi;

            int width = Screen.width;
            int height = Screen.height;

            int[] portraid = new int[4] { 728, 90, 320, 50};
            TypeDevice[] type = new TypeDevice[2]{ TypeDevice.TD_Tablet, TypeDevice.TD_Smartphone};

            for (int i = 0; i < 2; i++) {
                if (portraid[(i*2)+0] * density <= width && portraid[(i*2)+1] * density <= height)
                    return type[i];
            }

            return TypeDevice.TD_Tablet;
        }
    }
}
