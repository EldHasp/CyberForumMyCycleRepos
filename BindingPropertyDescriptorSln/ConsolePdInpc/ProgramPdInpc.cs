using System;

namespace ConsolePdInpc
{
	class ProgramPdInpc
	{
		static void Main(string[] args)
		{
			// Создание источника данных (ViewModel)
			SourceClassPdInpc source = new SourceClassPdInpc();

			// Создание двух Представлений для одного источника
			TargetClassPdInpc target1 = new TargetClassPdInpc("Первый", source, nameof(source.Text));
			TargetClassPdInpc target2 = new TargetClassPdInpc("Второй", source, nameof(source.Text));
			TargetClassPdInpc target3 = new TargetClassPdInpc("Третий", source, nameof(source.Length));

			// Цикл для изменения Представления и Источника
			do
			{
				Console.WriteLine();

				// Ввод данных в привязку
				Console.Write("Введите новое значение для target1.Value: ");
				target1.Value = Console.ReadLine();

				// Ввод данных в источник
				Console.Write("Введите новое значение для source.Text: ");
				source.Text = Console.ReadLine();

				Console.Write("Нажмите Enter для Продолжения или Esc для Выхода.....");
			} while (Console.ReadKey().Key != ConsoleKey.Escape);

		}
	}

}
