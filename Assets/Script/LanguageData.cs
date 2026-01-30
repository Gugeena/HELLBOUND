using Discord;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[System.Serializable]

public class LanguageData : MonoBehaviour
{
    public enum Languages {EN = 0, DE = 1, PG = 2, IT = 3, ES = 4, FR = 5, RU = 6}

    public static Languages currLanguage;
    public Languages unStaticCurrLangauge;

    private static Dictionary<string, string> English, German, Port, Italian, Spanish, French;
    public static Dictionary<string, string>[] TextData = new Dictionary<string, string>[7];

    private Dictionary<string, string[]> EnglishRaphael, GermanRaphael, PortRaphael, ItalianRaphael, SpanishRaphael, FrenchRaphael;

    public static LanguageData instance;

    public static Action onLanguageChange, onDeath;

    public Dropdown dropdown;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            //Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        English = new Dictionary<string, string>();
        German = new Dictionary<string, string>();
        Port = new Dictionary<string, string>();    
        Italian = new Dictionary<string, string>();
        Spanish = new Dictionary<string, string>();
        French = new Dictionary<string, string>();

        EnglishRaphael = new Dictionary<string, string[]>();
        GermanRaphael = new Dictionary<string, string[]>();
        PortRaphael = new Dictionary<string, string[]>();
        ItalianRaphael = new Dictionary<string, string[]>();
        SpanishRaphael = new Dictionary<string, string[]>();
        FrenchRaphael = new Dictionary<string, string[]>();

        fillDictionaries();

        TextData[0] = English;
        TextData[1] = German;
        TextData[2] = Port;
        TextData[3] = Italian;
        TextData[4] = Spanish;
        TextData[5] = French;
    }

    private void Start()
    {
        load();
    }

    private void fillDictionaries()
    {
        English.Add("start_game", "Start Game");
        English.Add("settings", "Settings");
        English.Add("discord", "Discord");
        English.Add("quit_game", "Quit Game");
        English.Add("tenth_layer_hell", "Tenth Layer of Hell");

        English.Add("jump", "Jump");
        English.Add("dash", "Dash");
        English.Add("attack", "Attack");
        English.Add("special", "Special");
        English.Add("drop", "Drop");
        English.Add("slide", "Slide");
        English.Add("music", "Music");
        English.Add("style_points", "Style points");
        English.Add("replay_tutorial", "Replay tutorial");

        English.Add("press_e_ascend", "Press E to Ascend");

        English.Add("banished_souls_eradicated", "Banished souls eradicated:");
        English.Add("damage_received", "Damage Received:");
        English.Add("time", "Time:");
        English.Add("eradication_notice", "Eradication Notice");

        English.Add("eradication_postponed", "Eradication postponed for now...");
        English.Add("return_menu", "Return to main menu?");

        English.Add("yes", "Yes");
        English.Add("no", "No");

        English.Add("first_note", "Whatever goes around, comes around.\nUse it to your advantage.\nLooped attacks grant \"Style Points\".\nBeware, enemies can exploit it as well.");
        English.Add("second_note", "Special moves grant bonus \"Style Points\".\nProlonged weapon use drains Style Points.\nMastering style brings you closer to enlightenment.\n Kill demons in vicinity to replenish life force.\nReach angelhood to face the exiled one.");

        English.Add("credits",
"A GAME MADE BY\nNINETY-NINE STUDIOS\n\n" +
"LEAD PROGRAMMERS\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"ASSISTANT PROGRAMMER\nIRAKLI MDINARADZE\n\n" +
"LEAD ARTIST\nIRAKLI MDINARADZE\n\n" +
"ASSISTANT ARTIST\nLUKA TSERTSVADZE\n\n" +
"MUSIC AND SOUNDS\nLEVAN VLASOVI\n\n" +
"LEAD ANIMATOR\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ASSISTANT ANIMATOR\nLASHA-GIORGI GUGENISHVILI\n\n" +
"SPECIAL THANKS TO\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        English.Add("the", "THE");
        English.Add("first", "FIRST");
        English.Add("exile", "EXILE");

        English.Add("signature", "Sincerely,\nDemon next door.");

        English.Add("thank_you", "BUT MOST IMPORTANTLY\nTHANK YOU FOR PLAYING OUR GAME");

        English.Add("quit_desktop", "Quit to desktop?");

        German.Add("start_game", "Spiel starten");
        German.Add("settings", "Einstellungen");
        German.Add("discord", "Discord");
        German.Add("quit_game", "Spiel beenden");
        German.Add("tenth_layer_hell", "Zehnte Ebene der Holle");

        German.Add("jump", "Springen");
        German.Add("dash", "Ausweichen");
        German.Add("attack", "Angreifen");
        German.Add("special", "Spezial");
        German.Add("drop", "Wegwerfen");
        German.Add("slide", "Rutschen");
        German.Add("music", "Musik");
        German.Add("style_points", "Stilpunkte");
        German.Add("replay_tutorial", "Tutorial wiederholen");

        German.Add("press_e_ascend", "Drucke E zum Aufsteigen");

        German.Add("banished_souls_eradicated", "Verbannte Seelen ausgeloscht:");
        German.Add("damage_received", "Erhaltener Schaden:");
        German.Add("time", "Zeit:");
        German.Add("eradication_notice", "Vernichtungsanzeige");

        German.Add("eradication_postponed", "Vernichtung vorerst verschoben...");
        German.Add("return_menu", "Zurueck zum hauptmenue?");

        German.Add("first_note", "Was herumgeht, kommt zurueck.\nNutze es zu deinem Vorteil.\nGeloopte Angriffe geben \"Style Points\".\nAchtung, auch Feinde koennen es ausnutzen.");
        German.Add("second_note", "Spezialbewegungen geben bonus \"Style Points\".\nLanger Waffengebrauch entzieht Style Points.\nDas Meistern des Stils bringt dich der Erleuchtung naeher.\nToete Daemonen in der Naehe, um Lebenskraft aufzufuellen.\nErreiche den Engelstatus, um dem Verbannten zu begegnen.");

        German.Add("the", "DER");
        German.Add("first", "ERSTE");
        German.Add("exile", "VERBANNTE");

        German.Add("credits",
"SPIEL ENTWICKELT VON\nNINETY-NINE STUDIOS\n\n" +
"LEITENDE PROGRAMMIERER\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"ASSISTENT PROGRAMMIERER\nIRAKLI MDINARADZE\n\n" +
"LEITENDER KUNSTLER\nIRAKLI MDINARADZE\n\n" +
"ASSISTENT KUNSTLER\nLUKA TSERTSVADZE\n\n" +
"MUSIK UND SOUND\nLEVAN VLASOVI\n\n" +
"LEITENDER ANIMATOR\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ASSISTENT ANIMATOR\nLASHA-GIORGI GUGENISHVILI\n\n" +
"BESONDERER DANK AN\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        German.Add("signature", "Mit freundlichen Gruessen,\nDemon nebenan.");

        German.Add("thank_you", "ABER AM WICHTIGSTEN\nDANKE, DASS DU UNSER SPIEL GESPIELT HAST");

        German.Add("yes", "Ja");
        German.Add("no", "Nein");

        German.Add("quit_desktop", "Spiel beenden?");

        Spanish.Add("start_game", "Iniciar juego");
        Spanish.Add("settings", "Configuracion");
        Spanish.Add("discord", "Discord");
        Spanish.Add("quit_game", "Salir del juego");
        Spanish.Add("tenth_layer_hell", "Décima Capa del Infierno");

        Spanish.Add("first_note", "Lo que va, vuelve.\nUsalo a tu favor.\nLos ataques en bucle otorgan \"Style Points\".\nCuidado, los enemigos tambien pueden usarlo.");
        Spanish.Add("second_note", "Los movimientos especiales otorgan bonus \"Style Points\".\nEl uso prolongado de armas drena Style Points.\nDominar el estilo te acerca a la iluminacion.\nMata demonios cercanos para restaurar la fuerza vital.\nAlcanza la angelitud para enfrentar al exiliado.");

        Spanish.Add("jump", "Saltar");
        Spanish.Add("dash", "Carerra");
        Spanish.Add("attack", "Atacar");
        Spanish.Add("special", "Especial");
        Spanish.Add("drop", "Soltar");
        Spanish.Add("slide", "Deslizar");
        Spanish.Add("music", "Música");
        Spanish.Add("style_points", "Puntos de estilo");
        Spanish.Add("replay_tutorial", "Repetir tutorial");

        Spanish.Add("press_e_ascend", "Presiona E para ascender");

        Spanish.Add("banished_souls_eradicated", "Almas desterradas erradicadas:");
        Spanish.Add("damage_received", "Dano recibido:");
        Spanish.Add("time", "Tiempo:");
        Spanish.Add("eradication_notice", "Aviso de erradicacion");

        Spanish.Add("eradication_postponed", "Erradicacion pospuesta por ahora...");
        Spanish.Add("return_menu", "Volver al menu principal?");

        Spanish.Add("the", "LA");
        Spanish.Add("first", "PRIMERA");
        Spanish.Add("exile", "EXILIADA");

        Spanish.Add("yes", "Si");
        Spanish.Add("no", "No");

        Spanish.Add("credits",
"UN JUEGO DE\nNINETY-NINE STUDIOS\n\n" +
"PROGRAMADORES PRINCIPALES\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"PROGRAMADOR ASISTENTE\nIRAKLI MDINARADZE\n\n" +
"ARTISTA PRINCIPAL\nIRAKLI MDINARADZE\n\n" +
"ARTISTA ASISTENTE\nLUKA TSERTSVADZE\n\n" +
"MUSICA Y SONIDOS\nLEVAN VLASOVI\n\n" +
"ANIMADOR PRINCIPAL\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ANIMADOR ASISTENTE\nLASHA-GIORGI GUGENISHVILI\n\n" +
"AGRADECIMIENTOS ESPECIALES A\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        Spanish.Add("signature", "Atentamente,\nDemonio de al lado.");

        Spanish.Add("thank_you", "PERO LO MAS IMPORTANTE\nGRACIAS POR JUGAR NUESTRO JUEGO");

        Spanish.Add("quit_desktop", "Salir al escritorio?");

        Port.Add("thank_you", "MAS O MAIS IMPORTANTE\nOBRIGADO POR JOGAR NOSSO JOGO");

        Port.Add("start_game", "Iniciar jogo");
        Port.Add("settings", "Configuracoes");
        Port.Add("discord", "Discord");
        Port.Add("quit_game", "Sair do jogo");
        Port.Add("tenth_layer_hell", "Decima Camada do Inferno");

        Port.Add("first_note", "O que vai, volta.\nUse isso a seu favor.\nAtaques em loop concedem \"Style Points\".\nCuidado, inimigos tambem podem usar isso.");
        Port.Add("second_note", "Movimentos especiais concedem bonus \"Style Points\".\nUso prolongado de armas drena Style Points.\nDominar o estilo te aproxima da iluminacao.\nMate demonios proximos para restaurar a forca vital.\nAlcance a angelitude para enfrentar o exilado.");

        Port.Add("jump", "Pular");
        Port.Add("dash", "Fugir");
        Port.Add("attack", "Atacar");
        Port.Add("special", "Especial");
        Port.Add("drop", "Soltar");
        Port.Add("slide", "Deslizar");
        Port.Add("music", "Musica");
        Port.Add("style_points", "Pontos de estilo");
        Port.Add("replay_tutorial", "Repetir tutorial");

        Port.Add("press_e_ascend", "Pressione E para ascender");

        Port.Add("banished_souls_eradicated", "Almas banidas erradicadas:");
        Port.Add("damage_received", "Dano recebido:");
        Port.Add("time", "Tempo:");
        Port.Add("eradication_notice", "Aviso de erradicacao");

        Port.Add("eradication_postponed", "Erradicacao adiada por enquanto...");
        Port.Add("return_menu", "voltar ao cardapio?");

        Port.Add("the", "A");
        Port.Add("first", "PRIMEIRA");
        Port.Add("exile", "EXILADA");

        Port.Add("yes", "Sim");
        Port.Add("no", "Nao");

        Port.Add("credits",
"JOGO CRIADO POR\nNINETY-NINE STUDIOS\n\n" +
"PROGRAMADORES PRINCIPAIS\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"PROGRAMADOR ASSISTENTE\nIRAKLI MDINARADZE\n\n" +
"ARTISTA PRINCIPAL\nIRAKLI MDINARADZE\n\n" +
"ARTISTA ASSISTENTE\nLUKA TSERTSVADZE\n\n" +
"MUSICA E SONS\nLEVAN VLASOVI\n\n" +
"ANIMADOR PRINCIPAL\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ANIMADOR ASSISTENTE\nLASHA-GIORGI GUGENISHVILI\n\n" +
"AGRADECIMENTOS ESPECIAIS A\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        Port.Add("signature", "Atenciosamente,\nDemonio ao lado.");

        Port.Add("quit_desktop", "Sair?");

        French.Add("start_game", "Demarrer le jeu");
        French.Add("settings", "Parametres");
        French.Add("discord", "Discord");
        French.Add("quit_game", "Quitter le jeu");
        French.Add("tenth_layer_hell", "Dixieme Couche de L’enfer");

        French.Add("first_note", "Ce qui part revient.\nUtilise-le a ton avantage.\nLes attaques en boucle donnent des \"Style Points\".\nAttention, les ennemis peuvent aussi en profiter.");
        French.Add("second_note", "Les mouvements speciaux accordent un bonus de \"Style Points\".\nL utilisation prolongee des armes draine les Style Points.\nMaitriser le style te rapproche de l illumination.\nTue des demons a proximite pour restaurer la force vitale.\nAtteins l etat angelique pour affronter l exile.");

        French.Add("jump", "Sauter");
        French.Add("dash", "Ruee");
        French.Add("attack", "Attaque");
        French.Add("special", "Special");
        French.Add("drop", "Lacher");
        French.Add("slide", "Glisser");
        French.Add("music", "Musique");
        French.Add("style_points", "Points de style");
        French.Add("replay_tutorial", "Revoir le tutoriel");

        French.Add("press_e_ascend", "Appuyez sur E pour ascendre");

        French.Add("banished_souls_eradicated", "Ames bannies eradiquees:");
        French.Add("damage_received", "Degats recus:");
        French.Add("time", "Temps:");
        French.Add("eradication_notice", "Avis d’eradication:");

        French.Add("eradication_postponed", "Eradication reportee pour le moment...");
        French.Add("return_menu", "Retour au menu principal?");

        French.Add("the", "LA");
        French.Add("first", "PREMIERE");
        French.Add("exile", "EXILEE");

        French.Add("signature", "Cordialement,Demon a cote.");

        French.Add("yes", "Oui");
        French.Add("no", "Non");

        French.Add("credits",
"UN JEU DE\nNINETY-NINE STUDIOS\n\n" +
"PROGRAMMEURS PRINCIPAUX\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"PROGRAMMEUR ASSISTANT\nIRAKLI MDINARADZE\n\n" +
"ARTISTE PRINCIPAL\nIRAKLI MDINARADZE\n\n" +
"ARTISTE ASSISTANT\nLUKA TSERTSVADZE\n\n" +
"MUSIQUE ET SONS\nLEVAN VLASOVI\n\n" +
"ANIMATEUR PRINCIPAL\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ANIMATEUR ASSISTANT\nLASHA-GIORGI GUGENISHVILI\n\n" +
"REMERCIEMENTS SPECIAUX A\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        French.Add("thank_you", "MAIS SURTOUT\nMERCI D AVOIR JOUE A NOTRE JEU");

        French.Add("quit_desktop", "Quitter vers le bureau?");

        Italian.Add("start_game", "Avvia gioco");
        Italian.Add("settings", "Impostazioni");
        Italian.Add("discord", "Discord");
        Italian.Add("quit_game", "Esci dal gioco");
        Italian.Add("tenth_layer_hell", "Decimo livello dell’inferno");

        Italian.Add("first_note", "Quello che va, ritorna.\nUsalo a tuo vantaggio.\nGli attacchi in loop danno \"Style Points\".\nAttenzione, anche i nemici possono sfruttarlo.");
        Italian.Add("second_note", "Le mosse speciali concedono bonus \"Style Points\".\nL uso prolungato delle armi consuma Style Points.\nPadroneggiare lo stile ti avvicina all illuminazione.\nUccidi demoni nelle vicinanze per ripristinare la forza vitale.\nRaggiungi lo stato angelico per affrontare l esiliato.");

        Italian.Add("jump", "Saltare");
        Italian.Add("dash", "Scatto");
        Italian.Add("attack", "Attaccare");
        Italian.Add("special", "Speciale");
        Italian.Add("drop", "Lasciare");
        Italian.Add("slide", "Scivolare");
        Italian.Add("music", "Musica");
        Italian.Add("style_points", "Punti stile");
        Italian.Add("replay_tutorial", "Ripeti tutorial");

        Italian.Add("press_e_ascend", "Premi E per ascendere");

        Italian.Add("banished_souls_eradicated", "Anime bandite eradicate:");
        Italian.Add("damage_received", "Danni subiti:");
        Italian.Add("time", "Tempo:");
        Italian.Add("eradication_notice", "Avviso di eradicazione");

        Italian.Add("eradication_postponed", "Eradicazione rimandata per ora...");
        Italian.Add("return_menu", "Ritorna al menu principale?");

        Italian.Add("the", "LA");
        Italian.Add("first", "PRIMA");
        Italian.Add("exile", "ESILIATA");

        Italian.Add("credits",
"UN GIOCO DI\nNINETY-NINE STUDIOS\n\n" +
"PROGRAMMATORI PRINCIPALI\nLASHA-GIORGI GUGENISHVILI\nLEVAN VLASOVI\n\n" +
"PROGRAMMATORE ASSISTENTE\nIRAKLI MDINARADZE\n\n" +
"ARTISTA PRINCIPALE\nIRAKLI MDINARADZE\n\n" +
"ARTISTA ASSISTENTE\nLUKA TSERTSVADZE\n\n" +
"MUSICA E SUONI\nLEVAN VLASOVI\n\n" +
"ANIMATORE PRINCIPALE\nLEVAN VLASOVI\nIRAKLI MDINARADZE\n\n" +
"ANIMATORE ASSISTENTE\nLASHA-GIORGI GUGENISHVILI\n\n" +
"RINGRAZIAMENTI SPECIALI A\nMARIAM SIKMASHVILI\nGIORGI GELASHVILI\nSANDRO VLASOVI\nALEXANDER KOPALIANI\nPLAYTESTERS");

        Italian.Add("thank_you", "MA SOPRATTUTTO\nGRAZIE PER AVER GIOCATO IL NOSTRO GIOCO");

        Italian.Add("signature", "Cordiali saluti,\nDemone accanto.");

        Italian.Add("quit_desktop", "Esci al desktop?");

        Italian.Add("yes", "Si");
        Italian.Add("no", "No");

        EnglishRaphael.Add("raphael0", new string[] { "Be not afraid" }); // kaia
        EnglishRaphael.Add("raphael1", new string[] { "Be not afraid", "Do not falter", "Master your style", "And the path shall open before you" }); // kaia
        EnglishRaphael.Add("raphael2", new string[] { "Find use in what surrounds you", "High ground can bring victory"}); // kaia
        EnglishRaphael.Add("raphael3", new string[] { "Do not let arms decide your strength", "a true warrior uses what he's given","and throws it away as easily" });
        EnglishRaphael.Add("raphael4", new string[] { "Use your instinct, and you will survive", "Use your Mind, and you will thrive" }); // kaia

        GermanRaphael.Add("raphael0", new string[] { "Furchte dich nicht" });
        GermanRaphael.Add("raphael1", new string[] { "Furchte dich nicht", "Zogere nicht", "Meistere deinen Stil", "Und der Weg wird sich dir offnen" });
        GermanRaphael.Add("raphael2", new string[] { "Nutze, was dich umgibt", "Hoher gelegenes Gelande kann den Sieg bringen" });
        GermanRaphael.Add("raphael3", new string[] { "Waffen entscheiden nicht uber deine Starke", "Ein wahrer Krieger nutzt, was ihm gegeben wird", "und legt es ebenso leicht wieder ab" });
        GermanRaphael.Add("raphael4", new string[] { "Nutze deinen Instinkt, und du wirst uberleben", "Nutze deinen Verstand, und du wirst gedeihen" });

        SpanishRaphael.Add("raphael0", new string[] { "No temas" });
        SpanishRaphael.Add("raphael1", new string[] { "No temas", "No vaciles", "Domina tu estilo", "Y el camino se abrira ante ti" });
        SpanishRaphael.Add("raphael2", new string[] { "Encuentra utilidad en lo que te rodea", "La altura puede darte la victoria" });
        SpanishRaphael.Add("raphael3", new string[] { "Las armas no deciden tu fuerza", "Un verdadero guerrero usa lo que se le da", "y lo abandona con la misma facilidad" });
        SpanishRaphael.Add("raphael4", new string[] { "Usa tu instinto y sobrevivirás", "Usa tu mente y prosperaras" });

        FrenchRaphael.Add("raphael0", new string[] { "N’aie pas peur" });
        FrenchRaphael.Add("raphael1", new string[] { "N’aie pas peur", "Ne vacille pas", "Maitrise ton style", "Et le chemin s’ouvrira a toi" });
        FrenchRaphael.Add("raphael2", new string[] { "Trouve une utilite a ce qui t’entoure", "L'altitude peut vous mener a la victoire" });
        FrenchRaphael.Add("raphael3", new string[] { "Les armes ne definissent pas ta force", "Un vrai guerrier utilise ce qui lui est donne", "et le rejette tout aussi facilement" });
        FrenchRaphael.Add("raphael4", new string[] { "Suis ton instinct et tu survivras", "Utilise ton esprit et tu prospereras" });

        ItalianRaphael.Add("raphael0", new string[] { "Non temere" });
        ItalianRaphael.Add("raphael1", new string[] { "Non temere", "Non esitare", "Padroneggia il tuo stile,", "E il cammino si aprira davanti a te" });
        ItalianRaphael.Add("raphael2", new string[] { "Trova utilita in cio che ti circonda", "Un terreno elevato puo portarti alla vittoria" });
        ItalianRaphael.Add("raphael3", new string[] { "Le armi non decidono la tua forza", "Un vero guerriero usa cio che gli viene dato", "e lo abbandona con la stessa facilita" });
        ItalianRaphael.Add("raphael4", new string[] { "Segui il tuo istinto e sopravviverai", "Usa la tua mente e prospererai" });

        PortRaphael.Add("raphael0", new string[] { "Nao temas" });
        PortRaphael.Add("raphael1", new string[] { "Nao temas", "Nao vaciles", "Domina o teu estilo", "E o caminho se abrira para ti" });
        PortRaphael.Add("raphael2", new string[] { "Encontra utilidade no que te rodeia", "A posicao elevada pode trazer a vitoria." });
        PortRaphael.Add("raphael3", new string[] { "As armas nao definem a tua forca", "Um verdadeiro guerreiro usa o que lhe e dado","e abandona-o com a mesma facilidade" });
        PortRaphael.Add("raphael4", new string[] { "Usa o teu instinto e sobreviveras", "Usa a tua mente e prosperaras" });
    }

    public string getCurrentLangText(string key)
    {
        return TextData[(int)currLanguage][key];
    }

    public void setLanguage(int index)
    {
        currLanguage = (Languages)index;
        unStaticCurrLangauge = currLanguage;
        onLanguageChange();
        GlobalSettings globalsettings = SaveSystem.Load();
        globalsettings.currlanguage = (int)this.unStaticCurrLangauge;
        SaveSystem.Save(globalsettings);
    }

    public void load()
    {
        GlobalSettings globalSettings = SaveSystem.Load();
        if (globalSettings.currlanguage != null) currLanguage = (Languages)globalSettings.currlanguage;
        else currLanguage = 0;
        unStaticCurrLangauge = currLanguage;
        onLanguageChange();
        if(dropdown != null) dropdown.value = (int)currLanguage;
    }

    public void playSubtitles(int variation)
    {
        string toplay = "raphael" + variation;
        Dictionary<string, string[]> pullFrom = new Dictionary<string, string[]>();
        int currentLanguage = (int)currLanguage;

        if (currentLanguage == 0) pullFrom = EnglishRaphael;
        else if (currentLanguage == 1) pullFrom = GermanRaphael;
        else if (currentLanguage == 2) pullFrom = PortRaphael;
        else if (currentLanguage == 3) pullFrom = ItalianRaphael;
        else if (currentLanguage == 4) pullFrom = SpanishRaphael;
        else if (currentLanguage == 5) pullFrom = FrenchRaphael;

        float time = 0f;
        float beetwenTime = 1.54f;
        float specialCase = 0f;

        if (variation == 0)
        {
            time = 4.2f;
            beetwenTime = 2f;
        }
        else if (variation == 2)
        {
            time = 0f;
            beetwenTime = 2.5f;
        }
        else if (variation == 4)
        {
            time = 0f;
            beetwenTime = 3f;
        }
        else if (variation == 3)
        {
            time = 0f;
            beetwenTime = 2.4f;
            specialCase = 1.3f;
        }

        //else if (variation == 1) time = 0f;
        //else if (variation == 2) pullFrom = PortRaphael;
        //else if (variation == 3) pullFrom = ItalianRaphael;
        //else if (variation == 4) pullFrom = SpanishRaphael;
        //else if (variation == 5) pullFrom = FrenchRaphael;

        StartCoroutine(subtitlesScript.subtitleStart(pullFrom[toplay], time, beetwenTime, specialCase));
    }
}
