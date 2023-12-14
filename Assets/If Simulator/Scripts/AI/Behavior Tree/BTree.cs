using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    [CreateAssetMenu(fileName = "New BTree", menuName = "Scriptable Objects/Behavior Tree")]
    public class BTree : ScriptableObject
    {
        [field: SerializeField, HideInInspector]
        public RootNode Root { get; set; }

        /// <summary>
        /// The tree's blackboard.
        /// </summary>
        public Blackboard Blackboard { get; } = new();

        /// <summary>
        /// State of the tree.
        /// </summary>
        public NodeState State => Root.State;

        /// <summary>
        /// Clones the tree. Needed for running multiple instances of the same tree.
        /// </summary>
        public BTree Clone()
        {
            BTree clone = Instantiate(this);
            clone.Root = (RootNode)Root.DeepInitialize(clone.Blackboard);
            return clone;
        }

        /// <summary>
        /// Evaluates the tree if it is still running.
        /// </summary>
        public void Update()
        {
            if (State == NodeState.Running)
                Root.Evaluate();
        }

        #region Editor Utilities

        [field: SerializeField, HideInInspector]
        public List<Node> AllNodes { get; private set; } = new();

        public Node CreateNode(Type nodeType)
        {
            Node node = (Node)CreateInstance(nodeType);
            AllNodes.Add(node);
            node.name = nodeType.Name;
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return node;
        }

        public void DeleteNode(Node node)
        {
            AllNodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Checks if the tree is valid.
        /// </summary>
        /// <param name="message">If the tree is invalid, contains a message explaining why.</param>
        /// <returns>True if the tree is valid, false otherwise.</returns>
        public bool Validate(out string message)
        {
            message = string.Empty;

            if (Root == null)
            {
                message = "The tree has no root node.";
                return false;
            }

            if (Root.Child == null)
            {
                message = "The root node has no child.";
                return false;
            }

            foreach (var node in AllNodes)
            {
                if (node is DecoratorNode decorator)
                {
                    if (decorator.Child == null)
                    {
                        message = $"The decorator node {node.name} has no child.";
                        return false;
                    }
                }
                else if (node is CompositeNode composite)
                {
                    if (composite.Children.Length == 0)
                    {
                        message = $"The composite node {node.name} has no children.";
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion
    }
}
