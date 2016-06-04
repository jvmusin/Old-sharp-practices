using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class Brainfuck
    {
        public static void Run(string program, Func<int> read, Action<char> write)
        {
            var runner = new ProgramRunner(program);
            runner.RegisterCommand('<', b => b.StatePointer--);
            runner.RegisterCommand('>', b => b.StatePointer++);
            runner.RegisterCommand('+', b => b.State[b.StatePointer]++);
            runner.RegisterCommand('-', b => b.State[b.StatePointer]--);
            runner.RegisterCommand('.', b => write((char)b.State[b.StatePointer]));
            runner.RegisterCommand(',', b => b.State[b.StatePointer] = (byte)read());

            var pairBrackets = FindPairBrackets(program);
            Action<ProgramRunner, Func<bool>> jumpToPairBracket = (b, predicate) =>
            {
                if (predicate())
                    b.ProgramPointer = pairBrackets[b.ProgramPointer];
            };
            runner.RegisterCommand('[', b => jumpToPairBracket(b, () => b.State[b.StatePointer] == 0));
            runner.RegisterCommand(']', b => jumpToPairBracket(b, () => b.State[b.StatePointer] != 0));

            runner.Run();
        }

        private static Dictionary<int, int> FindPairBrackets(string program)
        {
            var brackets = new Dictionary<int, int>();
            var currentBrackets = new Stack<int>();
            for (var i = 0; i < program.Length; i++)
            {
                switch (program[i])
                {
                    case '[':
                        currentBrackets.Push(i);
                        break;
                    case ']':
                        var openedAt = currentBrackets.Pop();
                        brackets[i] = openedAt;
                        brackets[openedAt] = i;
                        break;
                }
            }
            return brackets;
        }
    }

    public class ProgramRunner
    {
        private Dictionary<char, Action<ProgramRunner>> Commands { get; }

        public byte[] State { get; }
        public int StatePointer { get; set; }

        private string Program { get; }
        public int ProgramPointer { get; set; }

        public ProgramRunner(string program)
        {
            State = new byte[30000];
            Program = program;

            Commands = new Dictionary<char, Action<ProgramRunner>>();
            RegisterConstants();
        }

        private void RegisterConstants()
        {
            Action<char> register = c => RegisterCommand(c, b => b.State[b.StatePointer] = (byte)c);
            for (var c = 'a'; c <= 'z'; c++) register(c);
            for (var c = 'A'; c <= 'Z'; c++) register(c);
            for (var c = '0'; c <= '9'; c++) register(c);
        }

        public void RegisterCommand(char name, Action<ProgramRunner> command)
        {
            Commands[name] = command;
        }

        public void Run()
        {
            for (ProgramPointer = StatePointer = 0; ProgramPointer < Program.Length; ProgramPointer++)
            {
                if (Commands.ContainsKey(Program[ProgramPointer]))
                    Commands[Program[ProgramPointer]](this);
                StatePointer = (StatePointer + State.Length) % State.Length;
            }
        }
    }
}
