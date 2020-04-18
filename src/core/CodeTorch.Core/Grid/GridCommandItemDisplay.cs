using System;

namespace CodeTorch.Core
{
    [Serializable]
    public enum GridCommandItemDisplay
    {
        //
        // Summary:
        //     There will be no command item.
        None = 0,
        //
        // Summary:
        //     The command item will be above the Telerik RadGrid
        Top = 1,
        //
        // Summary:
        //     The command item will be on the bottom of Telerik RadGrid
        Bottom = 2,
        //
        // Summary:
        //     The command item will be both on the top and bottom of Telerik RadGrid.
        TopAndBottom = 3
    }
}
