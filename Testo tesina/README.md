# Elaborato: Introduzione alle REST API e Architetture Web
## Definizione di REST API: principi fondamentali e caratteristiche
- Le Rest Api (Representational State Transfer) sono un insieme di regole e convenzioni per la progettazione, costruzione di moltissimi servizi web che permettono la comunicazione tra il cliente ed il server. Queste sono basate sun protocollo chiamato HTTP e si fondano su alcuni principi fondamentali delle REST API e sono:
* Stateless: ogni richiesta è indipendente e non conserva lo stato tra client e server.

* Risorse: ogni elemento (es. studente, corso, prodotto) è rappresentato come una risorsa accessibile tramite un URL.

* Metodi HTTP: le operazioni sulle risorse si effettuano con metodi standard come GET, POST, PUT, DELETE.

* Formato leggibile: le risposte sono generalmente in JSON o XML, facilmente interpretabili da sistemi e persone.

D'altronde le REST API sono semplici, scalabili e sono la maggior parte utilizzate nello sviluppo di progetti moderni. Tra le tante archittetture (come le REST) ne abbiamo altre tra le quali:
* SOAP : è un protocollo molto più rigido, usato per lo più in ambienti enterprise ,(ovvero la realtà di un'azienda ove l'organizzazione comprende tutti i sistemi e i controlli che la fanno funzionare), con supporto a transazioni e di sicurezza avanzata
* GraphQL : invece è un'alternativa del mondo moderno che permette al client di specificare esattaamente quali dati richiede, riducendo così il traffico e aumentando la flessibilità.

## Descrizione dell'Autenticazione e della sicurezza (API key, JWT)
* Le API devono essere protette per evitare accessi non autorizzati. Esistono diversi metodi per farlo:
Come abbiamo visto in un esempio sopra le API Key sono delle chiavi segrete per permoettono al client di inviarla ad ogni richiesta. Ad esempio la chiave nel progetto Universita2.0 è Access: BRTDF129012

* Il JWT (JSON Web Token) è un token firmato che contiene le informazioni sull'utente. Il vantaggio del JWT è che può contenere ruoli, scadenze e dati utente, ed è molto usato in sistemi distribuiti.











# Descrizione del Progetto


## Introduzione al progetto Università_2.0
Il progetto sviluppato Universita2.0 è una Web API REST progettato in C# con .NET 8. Il seguente è stato pensato per una gestione semplificata di un sistema universitario complesso. L'applicazione consente di gestire gli studenti, i corsi e l'autenticazione offrendo così un interfaccia completa e documentata tramite Swagger ( è un insieme di strumenti e una specifica standard che permette di descrivere, progettare, documentare e consumare API RESTful in modo interattivo e automatizzato) per testare e visualizzare tutti gli endpoint disponibili.
La struttura è semplificata e non utilizza un database relazionale: i dati sono gestiti in memoria, rendendo il progetto leggero, veloce e facilmente comprensibile. L’autenticazione è gestita tramite API Key, e ogni richiesta protetta richiede un token statico per essere accettata. 
## Descrizione dei comandi (endpoint)

### POST/login
Funzione: che permette l'autenticazione dell'utente (in questo caso degli studenti)
* Richiesta:
* {
  "username": "Dante",
  "password": "Alighieri",
}

* Risposta 
* {
  "token": "BRTDF129012"
}
Il token ricevuto va inserito direttamente nell’header Access per accedere agli altri endpoint.

### GET/profilo 
Funzione: restituisce informazioni sensibili dell’utente autenticato

* Richiesta 
* GET /profilo
Header: Access: BRTDF129012
* Risposta
* info sensibili utente

### GET/api/studenti
Funzione: restituisce tutta la lista degli studenti compresi e presenti.
 
* Risposta
* [
  {
    "nome": "Matteo",
    "cognome": "Russo",
    "matricola": "A011",
    "isIscritto": true,
    "corsi": [...]
  },
  ...
]

### POST/api/studenti
Funzione: Permette di aggiungere un nuovo studente.

* Richiesta
* {
  "nome": "Giulia",
  "cognome": "Bianchi",
  "matricola": "A012",
  "isIscritto": true,
  "corsi": [
    { "id": 1, "nome": "Matematica", "isDisponibile": true }
  ]
}

* Risposta
* 201 Created

### GET /api/studenti/{matricola}
Funzione: Restituisce i dati di uno studente specifico dentro la lista

* se noi richiediamo = GET /api/studenti/A011

* avremo una risposta
* {
  "nome": "Matteo",
  "cognome": "Russo",
  "matricola": "A011",
  "isIscritto": true,
  "corsi": [...]
}

### GET /api/studenti/{matricola}/corsi-disponibili
Funzione: Mostra tutti corsi disponibili per uno specifico studente

* GET /api/studenti/A011/corsi-disponibili

* Risposta
* [
  { "id": 1, "nome": "Educazione Civica", "isDisponibile": true }
]

### GET /api/studenti/iscritti
Funzione: restituisce solamente gli studenti iscritti

* Risposta
* [
  { "matricola": "A011", "isIscritto": true },
  ...
]

### GET /api/corsi
Funzione: restituisce la lista completa dei corsi

* Risposta
* [
  { "id": 1, "nome": "Matematica", "isDisponibile": true },
  ...
]

### POST /api/corsi
Funzione: Permette di aggiungere un nuovo corso

* Richiesta
* {
  "nome": "Informatica",
  "isDisponibile": true
}
* Risposta
* 201 Created

### GET /api/corsi/{id}
Funzione: restituisce tutti i dettagli di un corso specifico

* se noi chiediamo GET /api/corsi/5

* Risposta
* {
  "id": 5,
  "nome": "Informatica",
  "isDisponibile": true
}

### PUT /api/corsi/{id}
Funzione: Permette di modificare un corso esistente 

* Richiesta
* {
  "nome": "Informatica Avanzata",
  "isDisponibile": false
}

### DELETE /api/corsi/{id}
Funzione: Permette di eliminare un corso

* esempio DELETE /api/corsi/5
questo eliminerà il corso con l'id 5 

### GET /api/corsi/disponibili
Funzione: restituisce solamente i corsi disponibili

* Risposta
* [
  { "id": 1, "nome": "Matematica", "isDisponibile": true },
  ...
]

## Descrizione degli Stati HTTP 
Come in qualche esempio qui sopra abbiamo potuto notare alcuni stati HTTP che indicano l'esito di una richiesta fatta dal server. Questi sono fondamentali pe capire se un'operazione è andata a buon fine o se si è verificato un errore.



| Codice | Significato              | Descrizione                                                                 |
|--------|--------------------------|------------------------------------------------------------------------------|
| 200    | OK                       | La richiesta è andata a buon fine e il server ha restituito i dati richiesti. |
| 201    | Created                  | La risorsa è stata creata correttamente (es. dopo un POST).                  |
| 400    | Bad Request              | La richiesta è malformata o contiene dati errati.                            |
| 401    | Unauthorized             | L’utente non è autenticato (token mancante o errato).                        |
| 403    | Forbidden                | L’utente è autenticato ma non ha i permessi per accedere alla risorsa.       |
| 404    | Not Found                | La risorsa richiesta non esiste.                                             |
| 500    | Internal Server Error    | Errore generico del server.                                                  |

 Questi codici sopra riportati aiutano il client a capire se la richiesta è andata a buon fine o se è necessario intervenire.

# Conclusione
Il progetto sviluppato rappresenta un chiaro esempio pratico di come realizzare una REST API con la gestione delle risorse e dell'autenticazione degli utenti.
Pur essendo semplificato, questa permette di comprendere al meglio i concetti fondamentali di:

* Struttura di un’API REST

* Utilizzo degli endpoint

* Gestione dell’autenticazione

* Importanza degli stati HTTP

Il Progetto Universita2.0 dimostra che anche con una semplicissima base sia possibile sviluppare un sistema che sia scalabile, semplice, chiaro ed estendibile.
In un lontano futuro l'applicazione potrebbe evolversi, aggiornarsi con nuove feature, introducendo anche un database relazionale e un nuovo versioning per le API per supportare le nuove funzionalità.

# Tutte le righe rappresentano dalle 4 alle 6 pagine piene in formato A4
