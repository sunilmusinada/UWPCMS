using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CMS_Survey.Models
{
    public delegate void ButtonClicked(object sender);
    public class SCommand:ICommand
    {
        public string CommandName;
        public UserSurvey userSurvey;
        public static event ButtonClicked Clicked;
        public SCommand(UserSurvey _userSurvey,string cmdName)
        {
            this.CommandName = cmdName;
            this.userSurvey = _userSurvey;
        }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            Clicked(this);
        }
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var canExecuteChanged = CanExecuteChanged;
            if (canExecuteChanged != null)
                canExecuteChanged(this, e);
        }
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }
    }

    
}
