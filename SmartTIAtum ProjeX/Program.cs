// See https://aka.ms/new-console-template for more information
using System;
using SmartTIAtumProjeX.ViewModels;
using SmartTIAtumProjeX.Views;

namespace SmartTIAtumProjeX
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var mainViewModel = new MainViewModel();
            var consoleView = new ConsoleView(mainViewModel);
            consoleView.Start();
        }
    }
}
