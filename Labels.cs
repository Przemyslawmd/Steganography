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
        }
        
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        public void SetPol( ref Dictionary<String, String> menu )                            
        {
            menu = menuPl;                                     
        }

        /*********************************************************************************************************************************/
        
        public void SetEng( ref Dictionary<String, String> menu )
        {
            menu = menuEn;                                            
        }
        
        /********************************************************************************************************************************/
        /* MENU *************************************************************************************************************************/

        private void InitializeMenu()
        {
            menuPl = new Dictionary<String, String>();
            menuEn = new Dictionary<String, String>();
                        
            menuPl.Add("openGraphic", "Otwórz plik graficzny");
            menuEn.Add("openGraphic", "Open Graphical File");
            menuPl.Add("openFile", "Otwórz plik do ukrycia");
            menuEn.Add("openFile", "Open File to Cover");            
            menuPl.Add("coverT", "Ukryj tekst");
            menuEn.Add("coverT", "Cover Text");
            menuPl.Add("coverF", "Ukryj dane z pliku");
            menuEn.Add("coverF", "Cover Data from File");
            menuPl.Add("uncoverT", "Odkryj tekst");
            menuEn.Add("uncoverT", "Uncover Text");
            menuPl.Add("uncoverF", "Odkryj dane");
            menuEn.Add("uncoverF", "Uncover Data");                      
        }             
               
               
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/

        Dictionary<String, String> menuPl;
        Dictionary<String, String> menuEn;        
        
    }
}
