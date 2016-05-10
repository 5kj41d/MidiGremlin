using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MidiGremlin.Internal;

namespace MidiGremlin
{
    ///<summary>
    ///The class ParallelMusicCollection inherits from the class MusicObject.
    ///It consists of a list of MusicObjects that will be played with the same start-time. 
    ///</summary>
    public class ParallelMusicCollection : MusicObject, IList<MusicObject>
    {
        private List<MusicObject> _children = new List<MusicObject>();



        /// <summary>
        /// Creates a new instance of the ParallelMusicObject class, containing a number of MusicObjects.
        /// </summary>
        /// <param name="children">Any number of MusicObjects. These will start playing at the same time.</param>
        public ParallelMusicCollection (IEnumerable<MusicObject> children)
        {
            foreach(MusicObject m in children)
            {
                Add(m);
            }
        }
        
        
        
        /// <summary>
        /// Creates a new instance of the ParallelMusicObject class, containing a number of MusicObjects.
        /// Should have at least 1 parameter.
        /// </summary>
        /// <param name="children">A number of MusicObjects. These will start playing at the same time.</param>
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
        /// <param name="index">Index where it gets and sets MusicObject.</param>
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
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count => _children.Count;



        /// <summary>
        /// Indicates that the list is not read-only.
        /// </summary>
        public bool IsReadOnly => false;



        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add (MusicObject item)
        {
            _children.Add(item);
        }


        
        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear ()
        {
            _children.Clear();
        }


        
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains (MusicObject item)
        {
            return _children.Contains(item);
        }



        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
        public void CopyTo (MusicObject[] array, int arrayIndex)
        {
            _children.CopyTo(array, arrayIndex);
        }



        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<MusicObject> GetEnumerator ()
        {
            return _children.GetEnumerator();
        }


        
        /// <summary>
        /// Searches  for the specified MusicObject and returns the index of the first occurrence within the list.
        /// </summary>
        /// <param name="item">The MusicObject which the list is seaching for.</param>
        /// <returns>The index of the MusicObject searched for.</returns>
        public int IndexOf (MusicObject item)
        {
            return _children.IndexOf(item);
        }



        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert (int index, MusicObject item)
        {
            _children.Insert(index, item);
        }


        
        /// <summary>
        /// Removes the first occurrence of the object within the list.
        /// </summary>
        /// <param name="item"> Specified object which should be removed. </param>
        /// <returns>Returns a list without the spicified object.</returns>
        public bool Remove (MusicObject item)
        {
            return _children.Remove(item);
        }



        /// <summary>
        /// Removes the element at a specified index.
        /// </summary>
        /// <param name="index">Index where the element to be removed is.</param>
        public void RemoveAt (int index)
        {
            _children.RemoveAt(index);
        }



        /// <summary>
        /// Returns the full contents of this MusicObject as SingleBeats.
        /// These are modified by the octave of the instrument that played them.
        /// </summary>
        /// <param name="playedBy">The instrument that requests the children.</param>
        /// <param name="startTime">The time at which the SingleBeats should start playing.</param>
        /// <returns>The full contents of this MusicObject as SingleBeats.</returns>
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



        /// <summary>
        /// Projects all music objects of specified type into a <see cref="MusicObject"/> of the same structure.
        /// </summary>
        /// <typeparam name="T">The MusicObject subtype to modify.</typeparam>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A <see cref="MusicObject"/> of identical structure that is the result of invoking the transform function of all elements of type T.</returns>
        public override MusicObject Select<T>(Func<T, T> selector)
        {
            List<MusicObject> resultChildren = _children
                .Select(x => x.Select(selector))    //Make sure to call MusicObject's Select on all composite children also.
                .ToList();
            ParallelMusicCollection result = new ParallelMusicCollection(resultChildren);

            if (this is T)
                return selector(result as T);
            else
                return result;
        }


        
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return _children.GetEnumerator();
        }
    }
}