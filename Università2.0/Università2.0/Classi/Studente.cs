namespace Università2._0.Classi
{

        public class Studente
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public string? Cognome { get; set; }
            public string? Matricola { get; set; }
            public bool IsIscritto { get; set; }
            public List<Corso> Corsi { get; set; } = new List<Corso>();
        }
    }
