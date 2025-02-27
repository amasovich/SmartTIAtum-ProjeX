using SmartTIAtumProjeX.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmartTIAtumProjeX.Views
{
    public class ConsoleView
    {
        private readonly MainViewModel _viewModel;

        public ConsoleView(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            // Подписка на изменения, чтобы при изменении данных сразу обновлять отображение
            _viewModel.PropertyChanged += (s, e) => Render();
        }

        public void Render()
        {
            Console.Clear();
            Console.WriteLine("=== SmartTIAtum ProjeX ===");
            Console.WriteLine($"Сообщение: {_viewModel.WelcomeMessage}");
        }

        public void Start()
        {
            Render();
            while (true)
            {
                Console.WriteLine("\nВведите новое сообщение или 'exit' для выхода:");
                var input = Console.ReadLine();
                if (input?.ToLower() == "exit")
                    break;
                _viewModel.WelcomeMessage = input;
            }
        }
    }
}

