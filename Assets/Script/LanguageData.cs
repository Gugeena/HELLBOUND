using Discord;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageData : MonoBehaviour
{
    public enum Languages {EN = 0, DE = 1, PG = 2, IT = 3, ES = 4, FR = 5, RU = 6}

    public static Languages currLanguage;

    private static Dictionary<string, string> English, German, Port, Italian, Spanish, French;
    public static Dictionary<string, string>[] TextData = new Dictionary<string, string>[7];

    private Dictionary<string, string[]> EnglishRaphael, GermanRaphael, PortRaphael, ItalianRaphael, SpanishRaphael, FrenchRaphael;

    public static LanguageData instance;

    public static Action onLanguageChange;

    private void Awake()
    {
        instance = this;

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
        setLanguage((int)Languages.EN);
    }

    private void fillDictionaries()
    {
        English.Add("start_game", "Start Game");
        English.Add("settings", "Settings");
        English.Add("discord", "Discord");
        English.Add("quit_game", "Quit Game");
        English.Add("tenth_layer_hell", "Tenth layer of hell");

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

        English.Add("banished_souls_eradicated", "Banished souls eradicated");
        English.Add("damage_received", "Damage Received");
        English.Add("time", "Time");
        English.Add("eradication_notice", "Eradication Notice");

        English.Add("eradication_postponed", "Eradication postponed for now");
        English.Add("quit_to_main_menu", "Quit to Main Menu");
        German.Add("start_game", "Spiel starten");
        German.Add("settings", "Einstellungen");
        German.Add("discord", "Discord");
        German.Add("quit_game", "Spiel beenden");
        German.Add("tenth_layer_hell", "Zehnte Ebene der Hölle");

        German.Add("jump", "Springen");
        German.Add("dash", "Ausweichen");
        German.Add("attack", "Angreifen");
        German.Add("special", "Spezial");
        German.Add("drop", "Wegwerfen");
        German.Add("slide", "Rutschen");
        German.Add("music", "Musik");
        German.Add("style_points", "Stilpunkte");
        German.Add("replay_tutorial", "Tutorial erneut spielen");

        German.Add("press_e_ascend", "Drücke E zum Aufsteigen");

        German.Add("banished_souls_eradicated", "Verbannte Seelen ausgelöscht");
        German.Add("damage_received", "Erhaltener Schaden");
        German.Add("time", "Zeit");
        German.Add("eradication_notice", "Vernichtungsanzeige");

        German.Add("eradication_postponed", "Vernichtung vorerst verschoben");
        German.Add("quit_to_main_menu", "Zurück zum Hauptmenü");
        Spanish.Add("start_game", "Iniciar juego");
        Spanish.Add("settings", "Configuración");
        Spanish.Add("discord", "Discord");
        Spanish.Add("quit_game", "Salir del juego");
        Spanish.Add("tenth_layer_hell", "Décima capa del infierno");

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

        Spanish.Add("banished_souls_eradicated", "Almas desterradas erradicadas");
        Spanish.Add("damage_received", "Daño recibido");
        Spanish.Add("time", "Tiempo");
        Spanish.Add("eradication_notice", "Aviso de erradicación");

        Spanish.Add("eradication_postponed", "Erradicación pospuesta por ahora");
        Spanish.Add("quit_to_main_menu", "Salir al menú principal");
        Port.Add("start_game", "Iniciar jogo");
        Port.Add("settings", "Configurações");
        Port.Add("discord", "Discord");
        Port.Add("quit_game", "Sair do jogo");
        Port.Add("tenth_layer_hell", "Décima camada do inferno");

        Port.Add("jump", "Pular");
        Port.Add("dash", "Avanço rápido");
        Port.Add("attack", "Atacar");
        Port.Add("special", "Especial");
        Port.Add("drop", "Soltar");
        Port.Add("slide", "Deslizar");
        Port.Add("music", "Música");
        Port.Add("style_points", "Pontos de estilo");
        Port.Add("replay_tutorial", "Repetir tutorial");

        Port.Add("press_e_ascend", "Pressione E para ascender");

        Port.Add("banished_souls_eradicated", "Almas banidas erradicadas");
        Port.Add("damage_received", "Dano recebido");
        Port.Add("time", "Tempo");
        Port.Add("eradication_notice", "Aviso de erradicação");

        Port.Add("eradication_postponed", "Erradicação adiada por enquanto");
        Port.Add("quit_to_main_menu", "Sair para o menu principal");
        French.Add("start_game", "Démarrer le jeu");
        French.Add("settings", "Paramètres");
        French.Add("discord", "Discord");
        French.Add("quit_game", "Quitter le jeu");
        French.Add("tenth_layer_hell", "Dixième couche de l’enfer");

        French.Add("jump", "Sauter");
        French.Add("dash", "Ruée");
        French.Add("attack", "Attaquer");
        French.Add("special", "Spécial");
        French.Add("drop", "Lâcher");
        French.Add("slide", "Glisser");
        French.Add("music", "Musique");
        French.Add("style_points", "Points de style");
        French.Add("replay_tutorial", "Rejouer le tutoriel");

        French.Add("press_e_ascend", "Appuyez sur E pour ascendre");

        French.Add("banished_souls_eradicated", "Âmes bannies éradiquées");
        French.Add("damage_received", "Dégâts reçus");
        French.Add("time", "Temps");
        French.Add("eradication_notice", "Avis d’éradication");

        French.Add("eradication_postponed", "Éradication reportée pour le moment");
        French.Add("quit_to_main_menu", "Quitter vers le menu principal");
        Italian.Add("start_game", "Avvia gioco");
        Italian.Add("settings", "Impostazioni");
        Italian.Add("discord", "Discord");
        Italian.Add("quit_game", "Esci dal gioco");
        Italian.Add("tenth_layer_hell", "Decimo livello dell’inferno");

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

        Italian.Add("banished_souls_eradicated", "Anime bandite eradicate");
        Italian.Add("damage_received", "Danni subiti");
        Italian.Add("time", "Tempo");
        Italian.Add("eradication_notice", "Avviso di eradicazione");

        Italian.Add("eradication_postponed", "Eradicazione rimandata per ora");
        Italian.Add("quit_to_main_menu", "Torna al menu principale");


        EnglishRaphael.Add("raphael0", new string[] { "Be not afraid" });
        EnglishRaphael.Add("raphael1", new string[] { "Be not afraid", "Do not falter", "Master your style,", "And the path shall open for you" });
        EnglishRaphael.Add("raphael2", new string[] { "Be not afraid" });
        EnglishRaphael.Add("raphael3", new string[] { "Be not afraid" });
        EnglishRaphael.Add("raphael4", new string[] { "Be not afraid" });


    }
    public string getCurrentLangText(string key)
    {
        return TextData[(int)currLanguage][key];
    }



    public void setLanguage(int index)
    {
        currLanguage = (Languages)index;
        onLanguageChange();
    }
}
