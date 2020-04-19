using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Blazor
{
    public class ContentBase: ComponentBase
    {
        protected RenderFragment renderFragment;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        //[Inject]
        //public IConfigurationStore ConfigurationStore { get; set; }



        protected override Task OnInitializedAsync()
        {
            //var app = ConfigurationStore.GetItem<App>("App");

            //renderFragment = CreateDynamicComponent;
            return Task.CompletedTask;
        }

        private void CreateDynamicComponent(RenderTreeBuilder builder)// => builder =>
        {
            //builder.OpenComponent(0, typeof(MainLayout2));
            builder.AddAttribute(1, "Body", (RenderFragment)((builder2) =>
            {
                builder2.AddContent(2, "testing");
            }));
            //builder.CloseComponent();


        }

    }
}
