using System;
using System.Collections.Generic;
using System.Text;

namespace Stegan
{
    class Labels
    {           
        public Labels()
        {
            InitializeMenu();
            InitializeHelp();
            InitializeMessages();
            InitializeAbout();
            InitializeSettings();            
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        public void SetPol( ref Dictionary<String, String> menu, ref Dictionary<String, String> help, ref Dictionary<String, String> mes, 
                            ref Dictionary<String, String> about, ref Dictionary<String, String> set)
        {
            menu = menuPl;
            help = helpPl;
            mes = mesPl;
            about = aboutPl;
            set = setPl;            
        }

        /*********************************************************************************************************************************/
        
        public void SetEng( ref Dictionary<String, String> menu, ref Dictionary<String, String> help, ref Dictionary<String, String> mes,
                            ref Dictionary<String, String> about, ref Dictionary<String, String> set)
        {
            menu = menuEn;
            help = helpEn;
            mes = mesEn;
            about = aboutEn;
            set = setEn;            
        }
        
        /********************************************************************************************************************************/
        /* MENU *************************************************************************************************************************/

        private void InitializeMenu()
        {
            menuPl = new Dictionary<String, String>();
            menuEn = new Dictionary<String, String>();

            menuPl.Add("file", "Plik");
            menuEn.Add("file", "File");
            menuPl.Add("openG", "Otwórz plik graficzny");
            menuEn.Add("openG", "Open Graphical File");
            menuPl.Add("openFile", "Otwórz plik do ukrycia");
            menuEn.Add("openFile", "Open File to Cover");
            menuPl.Add("saveG", "Zapisz plik graficzny");
            menuEn.Add("saveG", "Save Graphical File");
            menuPl.Add("saveFile", "Zapisz odkryte dane w pliku");
            menuEn.Add("saveFile", "Save Uncovered Data as File");
            menuPl.Add("remG", "Usuń plik graficzny");
            menuEn.Add("remG", "Remove Graphical File");
            menuPl.Add("remF", "Usuń dane odkryte/do ukrycia");
            menuEn.Add("remF", "Remove Uncovered/to Cover Data");
            menuPl.Add("remT", "Usuń tekst w kontrolce");
            menuEn.Add("remT", "Remove Text in Control");

            menuPl.Add("act", "Akcja");
            menuEn.Add("act", "Action");
            menuPl.Add("coverT", "Ukryj tekst");
            menuEn.Add("coverT", "Cover Text");
            menuPl.Add("coverF", "Ukryj dane z pliku");
            menuEn.Add("coverF", "Cover Data from File");
            menuPl.Add("uncoverT", "Odkryj tekst");
            menuEn.Add("uncoverT", "Uncover Text");
            menuPl.Add("uncoverF", "Odkryj dane");
            menuEn.Add("uncoverF", "Uncover Data");

            menuPl.Add("set", "Ustawienia");
            menuEn.Add("set", "Settings");
            menuPl.Add("info", "Informacja");
            menuEn.Add("info", "Information");
            menuPl.Add("about", "O aplikacji");
            menuEn.Add("about", "About");            
            menuPl.Add("help", "Pomoc");
            menuEn.Add("help", "Help");
        }

        /*******************************************************************************************************************************/
        /* HELP ************************************************************************************************************************/

        private void InitializeHelp()
        {
            helpEn = new Dictionary<string, string>();
            helpPl = new Dictionary<string, string>();

            String anchor = "<span style='line-height:200%; font-weight:bold;'>";
            String bold = "<p style='margin-bottom:0px;'><span style='font-weight:bold;'>";
            String navy = "<span style='color:navy; '>";
            String p8 = "<p style='margin-top:8px;'>";

            // COVERING DATA ////////////////////////////////////////////////////// 

            helpPl.Add("openG", "<p>" + anchor + "UKRYWANIE DANYCH W PLIKU GRAFICZNYM</span></p>" +
                "<p>Aby wczytać plik graficzny kliknij " + navy + "Otwórz plik graficzny</span> w menu Plik, dopuszczalne formaty to: JPG, PNG, GIF i BMP. </p>");
            
            helpPl.Add("coverText", bold + "Ukrywanie tekstu</span></p>" +
                p8 + "Wpisz tekst w polu po prawej stronie okna, a następnie kliknij " + navy + "Ukryj tekst</span> w menu " + navy + "Akcja</span>.</p>");
            
            helpPl.Add("coverData", bold + "Ukrywanie pliku</span></p>" + p8 + "Aby wczytać plik przeznaczony do ukrycia kliknij " + navy + "Wczytaj plik do ukrycia</span> w menu " + 
                navy + "Plik</span>, liczba wczytanych bajtów zostanie wyświetlona w polu w prawym dolnym rogu okna. " +
                "Następnie kliknij " + navy + "Ukryj plik</span> w menu " + navy + "Akcja</span>, nazwa pliku i jego rozszerzenie nie są ukrywane.</p>");

            helpEn.Add("openG", "<p>" + anchor + "COVERING DATA IN GRAPHIC</span></p>" +
                "<p>Click " + navy + " Open Graphical File</span> in menu " + navy + "File</span> to load a graphic. Formats which are allowed: JPG, PNG, GIF and BMP. ");
            
            helpEn.Add("coverText", bold + "Covering text</span></p>" +
                p8 + "Type text into a control at right side of window, then click " + navy + " Cover text</span> in menu " + navy + "Action</span>.");
            helpEn.Add("coverData", bold + "Covering file</span></p>" + p8 + "Click " + navy + " Open File to Cover</span> in menu " + navy + "File</span> to load a file intented to be covered. " + 
                "In case of success a number of read bytes will be displayed in a control in bottom right corner of window. " +
                "Next click " + navy + "Cover File</span> in menu " + navy + "Action</span>, what triggers action of covering, file name and its extension are not covered.</p>");
            
            // UNCOVERING DATA //////////////////////////////////////////////////////////////////

            helpPl.Add("unCover", "<hr/><p>" + anchor + "ODKRYWANIE DANYCH Z GRAFIKI</span></p>" +
                "<p>W grafice nie jest zapisywana informacja, czy zawiera ona ukryte dane, dlatego odkrywanie może być podjęte " +
                "wobec jakiejkolwiek grafiki. W przypadku grafiki nie zawierającej ukrytych danych, wynikiem prawdopodobnie będą 'niezrozumiałe' dane.</p>");

            helpPl.Add("unCoverT", bold + "Odkrywanie tekstu</span></p>" + p8 + "Kliknij " + navy + " Odkryj tekst </span>, tekst pojawi się w polu po prawej stronie okna.</p> ");

            helpPl.Add("unCoverD", bold + "Odkrywanie pliku</span></p>" + p8 + "Kliknij " + navy + " Odkryj dane</span>, dane zostaną zapisana w buforze. " +
                " a w polu w dolnym prawym rogu okna wyświetlona zostanie liczba odkrytych bajtów. " +
                "Aby zapisać odkryte bajty w formie pliku kliknij " + navy + "Zapisz odryte dane</span> w menu " + navy + "Plik</span>.</p>");

            helpEn.Add("unCover", "<hr/><p>" + anchor + "UNCOVERING DATA FROM GRAPHIC</span></p>" +
                "<p>There is no information in graphic whether it stores hidden data. Therefore an action of uncovering can be taken for " +
                "any graphic. When graphic has no hidden data inside, then probalby unlogical stream of bytes will be a result.</p>");

            helpEn.Add("unCoverT",  bold + "Uncovering text</span></p>" + p8 + "Click " + navy + " Uncover Text</span> in menu " + navy + "Action</span>, then text will appear in a control. </p>");

            helpEn.Add("unCoverD", bold + "Uncovering file</span></p>" +
                p8 + "Action " + navy + "Uncover File</span> in menu " + navy + "Action</span>, triggers uncovering data from graphic and buffered it, in control at right bottom corner will appear number of bytes in buffer. " +
                "Click " + navy + "Save Uncovered Data as File</span> to save buffered data as a file with indicated name and in indicated localization.</p>");
            
            // COMPRESSION ///////////////////////////////////////////////////////////////////////////////////////////////////////////////                     

            helpPl.Add("compress", "<hr/><p><span " + anchor + "KOMPRESJA</span></p>" +
                "<p>Zaznacz opcję " + navy + "Kompresja danych</span> w menu " + navy + "Ustawienia</span>, aby skompresować dane przed procesem ukrywania w grafice. " +
                "<p>Informacja o kompresji jest zapisywana w grafice, dlatego w trakcie odkrywania danych stan opcji nie jest brany pod uwagę. " +
                "Jeżeli dane zostały uprzednio skompresowane, to po odkryciu ich z grafki zostaną one zdekompresowane.");

            helpEn.Add("compress", "<hr/><p><span " + anchor + "COMPRESSION</span></p>" +
                "<p>Check an option " + navy + "Data compression</span> in menu "  + navy + "Settings</span> to invoke compression before covering. " +
                "<p>Information about compression is being stored in graphic while covering. " +
                "Therefore regardless of state of this option (checked or unchecked), compressed data will be decompressed after uncover.");
        }
        
        /******************************************************************************************************************************/
        /* MESSAGES *******************************************************************************************************************/

        private void InitializeMessages()
        {
            mesPl = new Dictionary<String, String>(8);
            mesEn = new Dictionary<String, String>(8);

            mesPl.Add("noText", "Brak tekstu do ukrycia.");
            mesEn.Add("noText", "There is no text to hide.");
            mesPl.Add("toManyData", "Za duża liczba bajtów dla wczytanej grafiki.");
            mesEn.Add("toManyData", "Too many data to be hidden into a loaded graphic.");
            mesPl.Add("dataCovered", "Dane zostały ukryte w pliku graficznym.");
            mesEn.Add("dataCovered", "Data was covered in a graphical file successfully.");
            mesPl.Add("mes6", "Niepoprawne dane wejściowe.");
            mesEn.Add("mes6", "Improper input data.");
            mesPl.Add("fileLoaded", "Wczytano plik: ");
            mesEn.Add("fileLoaded", "A file was loaded: ");
            mesPl.Add("numUncover", "Liczba odkrytych bajtów: ");
            mesEn.Add("numUncover", "Number of uncovered bytes: ");
            mesPl.Add("failureG", "Niepowodzenie wczytania grafiki");
            mesEn.Add("failureG", "Loading graphic failed");
            mesPl.Add("failureF", "Niepowodzenie wczytania pliku");
            mesEn.Add("failureF", "Loading file failed");

            // In messages, because there is no worth to maintain one pair of Dictionary for one content
            mesPl.Add("filterG", "Plik graficzny|*.jpg; *.jpeg; *.gif; *.bmp; *.png");
            mesEn.Add("filterG", "Graphical file|*.jpg; *.jpeg; *.gif; *.bmp; *.png");
        }        
        
        /******************************************************************************************************************************/
        /* ABOUT **********************************************************************************************************************/

        private void InitializeAbout()
        {
            aboutPl = new Dictionary<String, String>(4);
            aboutEn = new Dictionary<String, String>(4);            
            
            aboutPl.Add("description", "<pre>Aplikacja do steganografii</pre>");
            aboutEn.Add("description", "<pre>Steganography application</pre>");
            aboutPl.Add("author", "<pre>Autor:          Przemysław Madej, Kraków 2014</pre>");
            aboutEn.Add("author", "<pre>Author:         Przemysław Madej, Cracow 2014</pre>");
            aboutPl.Add("web", "<pre>Strona domowa:  http://przemeknet.pl</pre>");
            aboutEn.Add("web", "<pre>Home page:      http://przemeknet.pl</pre>");
            aboutPl.Add("license", "<pre>Licencja:       Freeware</pre>");
            aboutEn.Add("license", "<pre>License:        Freeware</pre>");
        }

        /******************************************************************************************************************************/
        /* SETTINGS********************************************************************************************************************/

        private void InitializeSettings()
        {
            setPl = new Dictionary<String, String>(3);
            setEn = new Dictionary<String, String>(3);
            
            setPl.Add("lan", "Język");
            setEn.Add("lan", "Language");
            setPl.Add("accept", "Akceptuj");
            setEn.Add("accept", "Accept");
            setPl.Add("compress", "Kompresja danych");
            setEn.Add("compress", "Data compression");
        }

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        Dictionary<String, String> menuPl;
        Dictionary<String, String> menuEn;
        Dictionary<String, String> aboutPl;
        Dictionary<String, String> aboutEn;
        Dictionary<String, String> mesPl;
        Dictionary<String, String> mesEn;
        Dictionary<String, String> helpEn;
        Dictionary<String, String> helpPl;
        Dictionary<String, String> setPl;
        Dictionary<String, String> setEn;
    }
}
