namespace Università2._0.Classi
{
   
        public class Corso
        {
            public int Id { get; set; }
            public string? Nome { get; set; }
            public bool IsDisponibile { get; set; }
            public List<Studente> Studenti { get; set; } = new List<Studente>();
        }
    }

