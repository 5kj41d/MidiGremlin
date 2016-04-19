using System;
using System.Collections;
using System.Collections.Generic;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class SequentialMusicCollection inherits from the class MusicObject.
    ///The class consist of a list of MusicObjects, which will be played apart from each other.
    ///The MusicObjects will be played in a certain order.
    ///</summary>
    public class SequentialMusicCollection : MusicObject, IList<MusicObject>
    {
        List<MusicObject> _children;
        public SequentialMusicCollection (IEnumerable<MusicObject> children)
        {
            foreach(MusicObject m in children)
            {
                Add(m);
            }
        }
        public SequentialMusicCollection (params MusicObject[] children)
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
            int tempTime = startTime;
            foreach (MusicObject m in _children)
            {
                if (m is Pause)
                {
                    tempTime += ((Pause)m).Duration;
                }
                else
                {
                    foreach (SingleBeat sb in m.GetChildren(playedBy, tempTime))
                    {
                        tempTime = Math.Max(tempTime, sb.ToneStartTime);
                        yield return sb;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _children.GetEnumerator();
        }
    }
}