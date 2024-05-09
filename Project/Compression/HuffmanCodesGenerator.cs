
using System.Collections.Generic;

namespace Steganography.Huffman
{
    class HuffmanCodesGenerator
    {
        public Dictionary< byte, List<Token>> CreateCodesDictionary( Node root )
        {
            GenerateCodes( root );
            return codesDictionary;
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private void GenerateCodes( Node node )
        {
            if ( node.Left != null )
            {
                code.Add( Token.Zero );
                GenerateCodes( node.Left );
            }
            if ( node.Right != null )
            {
                code.Add( Token.One );
                GenerateCodes( node.Right );
            }
            else
            {
                codesDictionary.Add( node.ByteValue, new List<Token>( code ));
            }

            if ( code.Count > 0 )
            {
                code.RemoveAt( code.Count - 1 );
            }
        }

        /**************************************************************************************/
        /**************************************************************************************/

        private readonly List<Token> code = new List<Token>();
        private readonly Dictionary<byte, List<Token>> codesDictionary = new Dictionary<byte, List<Token>>();
    }
}

