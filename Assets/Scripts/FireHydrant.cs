using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class FireHydrant : HoldActions
{
    private void Update()
    {
        if (held)
        {
            print("fire held");
        }
    }
}

