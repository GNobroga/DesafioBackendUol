using System.Xml.Linq;
using desafio_backend_uol.Models;

namespace desafio_backend_uol.Services;

public class CodinomeService
{
    private List<Codinome> Codinomes { get; set; } = new();

    public CodinomeService()
    {
        ReadOnJSON();
        ReadOnXML();
    }

    public List<Codinome> FindAllVingadores()
    {
        return Codinomes.Where(c => c.Grupo.Equals("Os Vingadores")).ToList();
    }

    public List<Codinome> FindAllLigaDaJustica()
    {
        return Codinomes.Where(c => c.Grupo.Equals("A Liga da Justiça")).ToList();
    }

    private void ReadOnXML()
    {
        try 
        {
            string url = "https://raw.githubusercontent.com/uolhost/test-backEnd-Java/master/referencias/liga_da_justica.xml";

            XDocument document = XDocument.Load(url);

            var parent = document.Descendants("codinomes").First();

            XNode? node = parent.Element("codinome");

            while (node != null)
            {
                XElement? element = node as XElement;
                Codinomes.Add(new Codinome { Nome = element!.Value, Grupo = "A Liga da Justiça"});
                node = node.NextNode;
            }
        }
        catch 
        {
            Console.WriteLine("Erro ao ler o arquivo liga_da_justica.xml");
        }
    }

    public void ReadOnJSON()
    {
        try 
        {
            string url = "https://raw.githubusercontent.com/uolhost/test-backEnd-Java/master/referencias/vingadores.json";
            using HttpClient httpClient = new();
            VingadorGrupo vingadorGrupo = httpClient.GetFromJsonAsync<VingadorGrupo>(url).Result!;
            Codinomes.AddRange(vingadorGrupo.Vingadores.Select(v => new Codinome { Nome = v.Codinome, Grupo = "Os Vingadores" }));
        }
        catch 
        {
            Console.WriteLine("Erro ao ler o arquivo vingadores.json");
        }
    }

}