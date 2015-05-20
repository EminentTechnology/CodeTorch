using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CodeTorch.Designer
{
    class ErrorManager
    {
        public static void HandleError(Exception ex)
        {
            Cursor.Current = Cursors.Default;
            string exceptionMessage = GetExceptionMessage(ex, 5);

            MessageBox.Show("The following error occurred:\n\n" + exceptionMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static string GetExceptionMessage(Exception ex, int LevelCount)
        {
            string retval = ex.Message;

            if ((ex.InnerException != null) && (LevelCount >= 0))
            {
                retval += "\n" + GetExceptionMessage(ex.InnerException, LevelCount - 1);
            }

            return retval;
        }
    }
}
