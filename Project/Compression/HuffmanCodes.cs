using System;
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Stegan
{
    class HuffmanCodes
    {
        /********************************************************************************/
        /* CREATE DICTIONARY WITH CODES *************************************************/        

        public Dictionary<byte, String> CreateCodesDictionary( NodeCompress root )
        {
            codes = new Dictionary<byte, string>();
            code = new List<char>();
            GenerateCodes( root, '1' );
            return codes;
        }

        /********************************************************************************/
        /* GENERATE CODES ***************************************************************/
        // Recursive method that traverses tree and generates codes 

        private void GenerateCodes( NodeCompress node, char token )
        {
            code.Add( token );

            if ( node.Left != null )
                GenerateCodes( node.Left, '0' );
            if ( node.Right != null )
                GenerateCodes( node.Right, '1' );
            else
                codes.Add( node.ByteValue, String.Concat( code ) );

            code.RemoveAt( code.Count - 1 );
        }

        /***********************************************************************************/
        /***********************************************************************************/

        // In some extreme cases, code may be very long, that it is used List<Char> instead of, 
        // for example, Long type 
        private List<char> code;

        private Dictionary<byte, string> codes;
    }
}
