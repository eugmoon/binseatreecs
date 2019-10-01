using System;
using System.Collections.Generic;

namespace binsearchtree
{
    class Node {
        public int value { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }

        public Node() { }
        public Node(int nodeValue) {
            this.value = nodeValue;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node root = new Node(20);
            root = insert(root, 8);
            root = insert(root, 22);
            root = insert(root, 20);    // insert repeated
            root = insert(root, 4);
            root = insert(root, 12);
            root = insert(root, 5);
            root = insert(root, 8);     // insert repeated
            root = insert(root, 10);
            root = insert(root, 14);
            root = insert(root, 22);    // insert repeated

            Console.WriteLine("\nOutput binary search tree: root");
            printBinTree(root);
            Console.WriteLine("Done!\n");

            root = delete(root, 20);

            Console.WriteLine("\nOutput binary search tree: root");
            printBinTree(root);
            Console.WriteLine("Done!\n");

            if (findTreeNode(root, 29) != null) {
                Console.WriteLine("Success!\n");
            } else {
                Console.WriteLine("Failed!\n");
            }

            if (findTreeNode(root, 5) != null) {
                Console.WriteLine("Success!\n");
            } else {
                Console.WriteLine("Failed!\n");
            }

            if (findParentNode(root, 20)!= null) {
                Console.WriteLine("Success!\n");
            } else {
                Console.WriteLine("Failed!\n");
            }

            if (findParentNode(root, 14)!= null) {
                Console.WriteLine("Success!\n");
            } else {
                Console.WriteLine("Failed!\n");
            }

            List<string> testList = serialize(root);
            Console.WriteLine("\nOutput serialized binary search tree: testList");
            printTreeList(testList);
            Console.WriteLine("Done!\n");

            Node testTree = deserialize(testList);
            Console.WriteLine("\nOutput binary search tree: testTree");
            printBinTree(testTree);
            Console.WriteLine("Done!\n");
        }

        static public Node insert(Node treeNode, int value) {
            Node currentNode = new Node();

            if (treeNode != null) {
                currentNode = treeNode;

                if (value < currentNode.value) {
                    currentNode.left = insert(currentNode.left, value);
                }
                else if (value > currentNode.value) {
                    currentNode.right = insert(currentNode.right, value);
                }
                else {      // value == currentNode.value
                    Console.WriteLine("Error: Did not insert {0}, already present in tree\n", value);
                }
            }
            else {      // treeNode == null
                currentNode.value = value;
            }

            return currentNode;
        }

        static public void printBinTree(Node treeNode) {
            if (treeNode != null) {
                Console.WriteLine("{0}", treeNode.value);
                printBinTree(treeNode.left);
                printBinTree(treeNode.right);
            }
            else {      // treeNode == null
                Console.WriteLine("N");
            }
        }

        static public Node delete(Node rootNode, int value) {
            Console.WriteLine("Deleting node with value {0} starting at node {1}", value, rootNode.value);

            if (rootNode != null) {
                if (value != rootNode.value) {
                    Node parentNode = findParentNode(rootNode, value);

                    if (parentNode != null) {     // found parent node
                        Node currentNode = new Node();
                        if (value < parentNode.value) {    // currentNode is left child of parent node
                            currentNode = parentNode.left;

                            if (currentNode.left == null) {     // currentNode has only right child or no children
                                parentNode.left = currentNode.right;
                            }
                            else if (currentNode.right == null) {   // currentNode has no right child
                                parentNode.left = currentNode.left;
                            }
                            else {      // currentNode has two children
                                Node replaceNode = findMaxNode(currentNode.left);
                                currentNode.value = replaceNode.value;
                                if (replaceNode == currentNode.left) {  // replaceNode is child of currentNode
                                    currentNode.left = null;
                                }
                                else {      // replaceNode is descendant of currentNode
                                    findParentNode(currentNode.left, replaceNode.value).right = null;
                                }
                            }
                        }
                        else if (value > parentNode.value) {     // currentNode is right child of parent node
                            currentNode = parentNode.right;

                            if (currentNode.left == null) {     // currentNode has only right child or no children
                                parentNode.right = currentNode.right;
                            }
                            else if (currentNode.right == null) {   // currentNode has no right child
                                parentNode.right = currentNode.left;
                            }
                            else {      // currentNode has two children
                                Node replaceNode = findMinNode(currentNode.right);
                                currentNode.value = replaceNode.value;
                                if (replaceNode == currentNode.right) {     // replaceNode is child of currentNode
                                    currentNode.right = null;
                                }
                                else {      // replaceNode is descendant of currentNode
                                    findParentNode(currentNode.right, replaceNode.value).left = null;
                                }
                            }
                        }
                    }
                }
                else {      // delete root node
                    if (rootNode.left != null) {
                        Node replaceNodeL = findMaxNode(rootNode.left);
                        replaceNodeL.left = rootNode.left;
                        replaceNodeL.right = rootNode.right;
                        if (replaceNodeL.value == rootNode.left.value) {
                            replaceNodeL.left = null;
                        }
                        else {
                            findParentNode(rootNode.left, replaceNodeL.value).right = null;
                        }
                        return replaceNodeL;
                    }
                    else if (rootNode.right != null) {
                        Node replaceNodeR = findMaxNode(rootNode.right);
                        replaceNodeR.left = rootNode.left;
                        replaceNodeR.right = rootNode.right;
                        if (replaceNodeR.value == rootNode.right.value) {
                            replaceNodeR.right = null;
                        }
                        else {
                            findParentNode(rootNode.right, replaceNodeR.value).left = null;
                        }
                        return replaceNodeR;
                    }
                }
            }

            return rootNode;
        }

        static public Node findParentNode(Node rootNode, int value) {
            Console.WriteLine("Finding parent of node with value {0} => {1}", value, rootNode.value);
            Node currentNode = rootNode;
            Stack<Node> nodeStack = new Stack<Node>();

            while (currentNode != null) {
                if (value != rootNode.value) {
                    if (value < currentNode.value) {
                        nodeStack.Push(currentNode);
                        currentNode = currentNode.left;
                    }
                    else if (value > currentNode.value) {
                        nodeStack.Push(currentNode);
                        currentNode = currentNode.right;
                    }
                    else {      // value == currentNode.value
                        currentNode = nodeStack.Pop();
                        break;
                    }
                }
                else {      // value == rootNode.value
                    Console.WriteLine("Value at current node equals {0}", rootNode.value);
                    return rootNode;
                }
            }

            if (currentNode != null) {
                Console.WriteLine("Value at current node equals {0}", currentNode.value);
            }
            else {
                Console.WriteLine("Error: {0} not in tree", value);
            }

            return currentNode;
        }

        static public Node findMinNode(Node treeNode) {
            Node currentNode = treeNode;
            Console.WriteLine("Finding min tree node => {0}", currentNode.value);

            while (currentNode.left != null) {
                currentNode = currentNode.left;
                Console.WriteLine("Finding min tree node => {0}", currentNode.value);
            }
            Console.WriteLine("Found: {0}", currentNode.value);

            return currentNode;
        }

        static public Node findMaxNode(Node treeNode) {
            Node currentNode = treeNode;
            Console.WriteLine("Finding max tree node => {0}", currentNode.value);

            while (currentNode.right != null) {
                currentNode = currentNode.right;
                Console.WriteLine("Finding max tree node => {0}", currentNode.value);
            }
            Console.WriteLine("Found: {0}", currentNode.value);

            return currentNode;
        }

        static public Node findTreeNode(Node treeNode, int value) {
            Node currentNode = new Node();

            if (treeNode != null) {
                if (treeNode.value != value) {      // value not equal initial node value
                    Node parentNode = findParentNode(treeNode, value);

                    if (parentNode != null) {      // found parent node
                        if (value < parentNode.value) {
                            currentNode = parentNode.left;
                        }
                        else if (value > parentNode.value) {
                            currentNode = parentNode.right;
                        }
                        else {      // value equal tree root node value
                            currentNode = parentNode;
                        }
                    }
                    else {      // parent node not found (== null)
                        return parentNode;
                    }
                }
                else {      // value equal initial node value
                    currentNode = treeNode;
                }
            }
            else {      // treeNode == null
                currentNode = null;
            }

            return currentNode;
        }

        static public List<string> serialize(Node treeNode) {
            List<string> serialList = new List<string>();

            if (treeNode != null) {     // add treeNode value to list (pre-order traversal)
                serialList.Add(treeNode.value.ToString());
                serialList.AddRange(serialize(treeNode.left));
                serialList.AddRange(serialize(treeNode.right));
            }
            else {      // treeNode == null, add sentinel value (N)
                serialList.Insert(0, "N");
            }

            return serialList;
        }

        static public Node deserialize(List<string> treeList) {
            Node tempNode = new Node();

            if (treeList.Count != 0) {
                if (treeList[0] != "N") {    // treeList not empty
                    tempNode.value = Convert.ToInt16(treeList[0]);
                    treeList.RemoveAt(0);
                    tempNode.left = deserialize(treeList);
                    treeList.RemoveAt(0);
                    tempNode.right = deserialize(treeList);
                }
                else {      // null node at front of treeList
                    tempNode = null;
                }
            }
            else {      // treeList is empty
                tempNode = null;
            }
            
            return tempNode;
        }

        static public void printTreeList(List<string> treeList) {
            foreach (string value in treeList) {
                Console.WriteLine("{0}", value);
            }
        }
    }
}