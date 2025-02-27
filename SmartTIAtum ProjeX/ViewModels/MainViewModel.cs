using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTIAtumProjeX.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string _welcomeMessage = "Привет, мир!";
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        // Дополнительную логику и команды можно добавить здесь.
    }
}
