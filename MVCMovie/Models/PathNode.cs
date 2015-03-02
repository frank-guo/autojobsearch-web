using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// The PathNode is to store the element position under its parent, i.e. the nth child, for all the nodes on the path to the root, i.e. document 
//If hasCommonParent is true that indicates the node on this level has the same parent for both node1 and node2, and node1 and node2 are the selected job title elements by users in SeletElement view.  
namespace MVCMovie.Models
{
    public class PathNode
    {
        public int ID {get; set;}
        public int position { get; set; }
        public bool hasCommonParent { get; set; }
    }
}