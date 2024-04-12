using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace CodeTorch.Designer.Wizards
{
    public class BaseWizardForm: Telerik.WinControls.UI.RadForm
    {
        public DockWindow DockWindow { get; set; }
        public RadTreeView Solution { get; set; }
        public Forms.MainForm2 MainForm { get; set; }

        public WizardPage CurrentPage { get; set; }
        public WizardPageCollection Pages { get; set; }
        public Stack<WizardStepState> WizardSteps { get; private set; } = new Stack<WizardStepState>();

        public int GetStepIndex(WizardPage Page)
        {
            if(Pages == null)
            {
                throw new NotImplementedException("Pages needs to be assigned in your Wizard constructor");
            }

            return Pages.IndexOf(Page);
        }

        public virtual IStepHandler GetStepHandler(int stepIndex)
        {
            throw new NotImplementedException("GetStepHandler needs to be implemented in your Wizard");
        }

        public virtual WizardPage GetInitialPage()
        {
            return Pages[0];
        }

        public virtual WizardPage GetNextPage()
        {
            throw new NotImplementedException("GetNextPage needs to be implemented in your Wizard");
        }

        public IStepHandler GetStepHandler(WizardPage Page)
        {
            var stepIndex = GetStepIndex(Page);
            return GetStepHandler(stepIndex);
        }

        

        public async Task OnWizardPrevious(object sender, WizardCancelEventArgs e, object stateObject)
        {
            try
            {
                if (WizardSteps.Count > 0)
                {
                    var handler = GetStepHandler(CurrentPage);
                    var success = await handler.OnPreviousClick(this, stateObject);

                    if (success)
                    {
                        WizardStepState state = WizardSteps.Pop();
                        CurrentPage = state.Step;
                    }
                }
                else
                {
                    CurrentPage = GetInitialPage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                e.Cancel = true;
            }
        }

        public async Task OnWizardNext(object sender, WizardCancelEventArgs e, object stateObject)
        {
            var success = true;
            try
            {
                var handler = GetStepHandler(CurrentPage);

                bool IsValid = await handler.PerformValidation(this, stateObject);

                if (IsValid)
                {
                    success = await handler.OnNextClick(this, stateObject);

                    if (success)
                    {
                        WizardStepState stepState = new WizardStepState();
                        stepState.Step = CurrentPage;
                        WizardSteps.Push(stepState);
                                                
                        CurrentPage = GetNextPage();

                        var nextPageHandler = GetStepHandler(CurrentPage);

                        await nextPageHandler.OnPageLoad(this, stateObject);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            Console.WriteLine("OnWizardNext: " + success);
            e.Cancel = true;
        }

        public async Task OnWizardFinish(object sender, EventArgs e, object stateObject)
        {
            try
            {
                var handler = GetStepHandler(CurrentPage);
                await handler.OnNextClick(this, stateObject);

                if ((Solution != null) && (MainForm != null))
                {
                    MainForm.RefreshAll();
                    MainForm.Build();
                }

                if (this.DockWindow != null)
                {
                    this.DockWindow.Close();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public Task OnWizardCancel(object sender, EventArgs e)
        {
            if (this.DockWindow != null)
            {
                this.DockWindow.Close();
            }
            else
            {
                this.Close();
            }

            return Task.CompletedTask;
        }
    }
}
