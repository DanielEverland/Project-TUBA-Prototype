using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBase : ScriptableObject
{
    public const string MENU_ROOT = "Inventory/";
    public const int MENU_ORDER = 106;
    
    protected class ItemCollection<T> : IEnumerable<T>
    {
        public ItemCollection(int slotCount)
        {
            Slots = new T[slotCount];
        }

        public T this[int index]
        {
            get
            {
                return Slots[index];
            }
            set
            {
                Slots[index] = value;
            }
        }
        
        public T[] Slots { get; private set; }

        public IEnumerator<T> GetEnumerator()
        {
            return GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Slots.GetEnumerator();
        }
    }
}