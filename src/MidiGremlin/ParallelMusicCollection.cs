using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class ParallelMusicCollection inherits from the class MusicObject.
    ///The class consist of a list of MusicObjects, which will be played simultaneously. 
    ///</summary>
    public class ParallelMusicCollection : MusicObject, IList<MusicObject>
    {
        List<MusicObject> _children;
        public ParallelMusicCollection (IEnumerable<MusicObject> children)
        {
            foreach(MusicObject m in children)
            {
                Add(m);
            }
        }
        public ParallelMusicCollection (params MusicObject[] children)
        {
            foreach (MusicObject m in children)
            {
                Add(m);
            }
        }

        public MusicObject this[int index]
        {
            get
            {
                return _children[index];
            }

            set
            {
                _children[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return _children.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add (MusicObject item)
        {
            _children.Add(item);
        }

        public void Clear ()
        {
            _children.Clear();
        }

        public bool Contains (MusicObject item)
        {
            return _children.Contains(item);
        }

        public void CopyTo (MusicObject[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<MusicObject> GetEnumerator ()
        {
            return _children.GetEnumerator();
        }

        public int IndexOf (MusicObject item)
        {
            return _children.IndexOf(item);
        }

        public void Insert (int index, MusicObject item)
        {
            _children.Insert(index, item);
        }

        public bool Remove (MusicObject item)
        {
            return _children.Remove(item);
        }

        public void RemoveAt (int index)
        {
            _children.RemoveAt(index);
        }

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, int startTime)
        {
            
            foreach (MusicObject m in _children)
            {
                
            }
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _children.GetEnumerator();
        }
    }
}