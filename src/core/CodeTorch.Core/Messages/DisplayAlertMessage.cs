using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Messages
{
    public sealed class DisplayAlertMessage
    {
        /// <summary>
        /// Gets the <see cref="ViewTargets"/> value indicating which view to show.
        /// </summary>
        //public ViewTargets ViewTarget { get; private set; }

        /// <summary>
        /// Gets the optional load arguments used by the view to load itself.
        /// </summary>
        public Object LoadArgs { get; private set; }

        public bool IsDismissable { get;  set; }

        public int AlertType { get; set; }
        public string Text { get; set; }

        public const int ALERT_DANGER = -1;
        public const int ALERT_WARNING = 0;
        public const int ALERT_INFO = 1;
        public const int ALERT_SUCCESS = 2;

        /// <summary>
        /// Constructor for ShowViewMessage.
        /// </summary>
        /// <param name="viewTarget">
        /// The <see cref="ViewTargets"/> value indicating which view to show.
        /// </param>
        public DisplayAlertMessage()
        {
            //ViewTarget = viewTarget;
        }

        /// <summary>
        /// Constructor for ShowViewMessage which provides optional load arguments used by
        /// the view to load itself.
        /// </summary>
        /// <param name="viewTarget">
        /// The <see cref="ViewTargets"/> value indicating which view to show.
        /// </param>
        /// <param name="loadArgs">
        /// Optional load arguments which will be used by the view to load itself.
        /// </param>
        ////public PerformSearchMessage(ViewTargets viewTarget, Object loadArgs)
        //    : this(viewTarget)
        //{
        //    LoadArgs = loadArgs;
        //}
    }
}
