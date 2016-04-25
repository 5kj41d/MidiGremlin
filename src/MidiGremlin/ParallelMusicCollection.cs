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
        List<MusicObject> _children = new List<MusicObject>();
        /// <summary>
        /// Adds a number of MusicObjects to parralellmusiclist 
        /// </summary>
        /// <param name="children">A number of MusicObjects</param>
        public ParallelMusicCollection (IEnumerable<MusicObject> children)
        {
            foreach(MusicObject m in children)
            {
                Add(m);
            }
        }
        /// <summary>
        /// Adds a number of MusicObjects to parralellmusiclist. Give it at least 1 parameter
        /// </summary>
        /// <param name="children">A number of MusicObjects.</param>
        public ParallelMusicCollection (params MusicObject[] children)
        {
            foreach (MusicObject m in children)
            {
                Add(m);
            }
        }
        /// <summary>
        ///  Gets and sets MusicObject from the indicated index.
        /// </summary>
        /// <param name="index">Indicated index where it gets and sets MusicObject</param>
        /// <returns></returns>
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
        /// <summary>
        /// Gets the number of elements contained in the list
        /// </summary>
        public int Count
        {
            get
            {
                return _children.Count;
            }
        }
        /// <summary>
        /// Checks if it is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Adds MusicObject to list
        /// </summary>
        /// <param name="item">Is a MusicObject</param>
        public void Add (MusicObject item)
        {
            _children.Add(item);
        }
        /// <summary>
        /// clears the list
        /// </summary>
        public void Clear ()
        {
            _children.Clear();
        }
        /// <summary>
        /// checks if it is contained in list
        /// </summary>
        /// <param name="item">Is a MusicObject</param>
        /// <returns></returns>
        public bool Contains (MusicObject item)
        {
            return _children.Contains(item);
        }
        /// <summary>
        /// copies to list
        /// </summary>
        /// <param name="array">Is a MusicObject array</param>
        /// <param name="arrayIndex">index of the array</param>
        public void CopyTo (MusicObject[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MusicObject> GetEnumerator ()
        {
            return _children.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf (MusicObject item)
        {
            return _children.IndexOf(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert (int index, MusicObject item)
        {
            _children.Insert(index, item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove (MusicObject item)
        {
            return _children.Remove(item);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt (int index)
        {
            _children.RemoveAt(index);
        }

        internal override IEnumerable<SingleBeat> GetChildren (Instrument playedBy, double startTime)
        {
            List<SingleBeat> sBeats = new List<SingleBeat>();
            foreach (MusicObject m in _children)
            {
                foreach (SingleBeat sb in m.GetChildren(playedBy, startTime))
                {
                    sBeats.Add(sb);
                }
            }
            sBeats = sBeats.OrderBy(sb => sb.ToneStartTime).ToList();

            foreach (SingleBeat sb in sBeats)
            {
                yield return sb;
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _children.GetEnumerator();
        }
    }
}