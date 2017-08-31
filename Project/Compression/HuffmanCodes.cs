
using System.Collections.Generic;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo( "Tests" )]

namespace Stegan
{
    class HuffmanCodes
    {
        /********************************************************************************/
        /* CREATE CODES DICTIONARY ******************************************************/        

        public Dictionary<byte, List<char>> CreateCodesDictionary( NodeCompress root )
        {
            codes = new Dictionary<byte, List<char>>();
            code = new List<char>();
            GenerateCodes( root, '1' );
            return codes;
        }

        /********************************************************************************/
        /* GENERATE CODES ***************************************************************/
        // Recursive method that traverses tree and generates codes 

        private void GenerateCodes( Node node, char token )
        {
            code.Add( token );

            if ( node.Left != null )
                GenerateCodes( node.Left, '0' );
            if ( node.Right != null )
                GenerateCodes( node.Right, '1' );
            else
                codes.Add( node.ByteValue, new List<char>( code ));

            code.RemoveAt( code.Count - 1 );
        }

        /***********************************************************************************/
        /***********************************************************************************/

        // In some extreme cases, code may be very long, this is the reason of using List<Char> instead of, 
        // for example, Long type 
        private List<char> code;

        private Dictionary<byte, List<char>> codes;
    }
}
