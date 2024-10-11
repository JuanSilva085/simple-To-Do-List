using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using Newtonsoft.Json;

public class Tarefa
{
    public string Descricao { get; set; }
    public DateTime DataVencimento { get; set; }
    public bool Concluida { get; set; }

    public Tarefa(string descricao, DateTime dataVencimento)
    {
        Descricao = descricao;
        DataVencimento = dataVencimento;
        Concluida = false;
    }

    public void MarcarComoConcluida()
    {
        Concluida = true;
    }

    public override string ToString()
    {
        return $"[{(Concluida ? "X" : " ")}] {Descricao} - Vence em: {DataVencimento.ToShortDateString()}";
    }
}

public class GerenciadorDeTarefas
{
    private List<Tarefa> tarefas = new List<Tarefa>();
    private const string Arquivo = "tarefas.json";

    public void AdicionarTarefa(Tarefa tarefa)
    {
        tarefas.Add(tarefa);
        SalvarTarefas();
    }

    public void ListarTarefas()
    {
        if (tarefas.Count == 0)
        {
            Console.WriteLine("Nenhuma tarefa cadastrada.");
            return;
        }

        for (int i = 0; i < tarefas.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tarefas[i]}");
        }
    }

    public void ExcluirTarefa(int indice)
    {
        if (indice >= 0 && indice < tarefas.Count)
        {
            tarefas.RemoveAt(indice);
            SalvarTarefas();
        }
        else
        {
            Console.WriteLine("Indice de tarefa inválido.");
        }
    }

    public void MarcarTarefaComoConcluida(int indice)
    {
        if (indice >= 0 && indice < tarefas.Count)
        {
            tarefas[indice].MarcarComoConcluida();
            SalvarTarefas();
        }
        else
        {
            Console.WriteLine("Tarefa inválida.");
        }
    }

    public void CarregarTarefas()
    {
        if (File.Exists(Arquivo))
        {
            string json = File.ReadAllText(Arquivo);
            tarefas = JsonConvert.DeserializeObject<List<Tarefa>>(json) ?? new List<Tarefa>();
        }
    }

    public void SalvarTarefas()
    {
        string json = JsonConvert.SerializeObject(tarefas, Formatting.Indented);
        File.WriteAllText(Arquivo, json);
    }
}


//interface

public class Program
{
    static void Main()
    {
        GerenciadorDeTarefas gerenciador = new GerenciadorDeTarefas();
        gerenciador.CarregarTarefas();


        while (true)
        {
            Console.WriteLine("\n--- Gerenciador de Tarefas ---");
            Console.WriteLine("1. Adicionar Tarefa");
            Console.WriteLine("2. Listar Tarefas");
            Console.WriteLine("3. Marcar tarefa como concluída");
            Console.WriteLine("4. Excluir Tarefa");
            Console.WriteLine("5. Sair");
            Console.WriteLine("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    Console.WriteLine("Descrição da tarefa: ");
                    string descricao = Console.ReadLine();
                    Console.Write("Data de vencimento (dd/mm/aaaa: ");
                    if(DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataVencimento))
                    {
                        gerenciador.AdicionarTarefa(new Tarefa(descricao, dataVencimento));
                    }
                    else
                    {
                        Console.WriteLine("Data inválida.");
                    }
                    break;
                case "2":
                    gerenciador.ListarTarefas();
                    break;
                case "3":
                    Console.Write("Número da tarefa para marcar como concluída: ");
                    if (int.TryParse(Console.ReadLine(), out int numeroConcluir))
                    {
                        gerenciador.MarcarTarefaComoConcluida(numeroConcluir - 1);
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida.");
                    }
                    break;
                case "4":
                    Console.Write("Número da tarefa para excluir: ");
                    if (int.TryParse(Console.ReadLine(), out int numeroExcluir))
                    {
                        gerenciador.ExcluirTarefa(numeroExcluir - 1);
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida.");
                    }
                    break;
                case "5":
                    Console.WriteLine("Saindo...");
                    return;
                default:
                    Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                    break;
            }
        }
    }
}