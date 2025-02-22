﻿/*
 * Project	    : Outlook 2003 Style Navigation Pane
 *
 * Author       : Muhammed ŞAHİN
 * eMail        : muhammed.sahin@gmail.com
 *
 * Description  : NavigateBarButton collection
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ProgrammersInc.Windows.Forms
{
    /// <summary>
    ///  NavigateBarButton collection
    /// </summary>
    public class NavigateBarButtonCollection : CollectionBase
    {

        #region Delegates
        internal delegate void OnButtonEventHandler(NavigateBarButtonEventArgs e);
        /// <summary>
        /// Occurs when remove button in collection
        /// </summary>
        internal event OnButtonEventHandler OnButtonAdded;

        /// <summary>
        /// Occurs when add new button in collection
        /// </summary>
        internal event OnButtonEventHandler OnButtonRemoved;
        #endregion

        #region Constructors

        private NavigateBar NavigationPane;

        public NavigateBarButtonCollection(NavigateBar tNavigateBar)
        {
            this.NavigationPane = tNavigateBar;
        }
        #endregion

        #region Indexer
        public NavigateBarButton this[int index]
        {
            get { return (NavigateBarButton)this.List[index]; }
            set
            {
                if (value is NavigateBarButton)
                {
                    this.List[index] = value;
                }
                else
                    throw new Exception("Value must be NavigateBarButton");
            }

        }
        #endregion

        #region Methods
        /// <summary>
        /// Add a new button in collection
        /// </summary>
        /// <param name="tButton">NavigateBarButton object</param>
        public virtual void Add(NavigateBarButton tButton)
        {
            // Only once
            if (!this.Contains(tButton))
                this.List.Add(tButton);
        }

        /// <summary>
        /// Agrega una colección de elementos a la colección actual.
        /// </summary>
        /// <param name="tButtons">Colección de elementos a agregarse.</param>
        public virtual void AddRange(NavigateBarButton[] tButtons)
        {
            foreach (NavigateBarButton nvb in tButtons)
                this.Add(nvb); 
        }

        /// <summary>
        /// Elimina un elemento dado de la lista actual.
        /// </summary>
        /// <param name="tButton">Elemento a ser eliminado.</param>
        public virtual void Remove(NavigateBarButton button)
        {
            if (this.Contains(button))
                this.List.Remove(button);
        }

        /// <summary>
        /// Remove exist a button from collection using key value
        /// </summary>
        /// <param name="tKey"></param>
        public virtual void RemoveByKey(string tKey)
        {
            NavigateBarButton nvb = this.FindByKey(tKey);
            if (nvb != null)
                this.Remove(nvb);
        }

        /// <summary>
        /// Insert a new button in collection
        /// </summary>
        /// <param name="tIndex">Index no</param>
        /// <param name="tButton">NavigateBarButton object</param>
        public virtual void Insert(int tIndex, NavigateBarButton tButton)
        {
            if (!this.Contains(tButton))
                this.List.Insert(tIndex, tButton);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);

            if (OnButtonAdded != null)
                OnButtonAdded(new NavigateBarButtonEventArgs(value as NavigateBarButton));

        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);

            if (OnButtonRemoved != null)
                OnButtonRemoved(new NavigateBarButtonEventArgs(value as NavigateBarButton));

        }

        /// <summary>
        /// Verifica si en la colección existe el elemento dado.
        /// </summary>
        /// <param name="tButton">Elemento a verificar.</param>
        /// <returns><c>true</c> si el elemento esta contenido en la colección,
        /// en otro caso retornará <c>false</c>.</returns>
        public virtual bool Contains(NavigateBarButton tButton)
        {
            return this.List.Contains(tButton);
        }

        /// <summary>
        /// Get index no in collection
        /// </summary>
        /// <param name="tButton">NavigateBarButton object</param>
        /// <returns>int</returns>
        public virtual int IndexOf(NavigateBarButton tButton)
        {
            return this.List.IndexOf(tButton);
        }
        #endregion

        #region Custom Methods
        /// <summary>
        /// Get displayed button count in panel
        /// </summary>
        /// <returns></returns>
        public int GetDisplayedItemCount()
        {
            int count = 0;
            foreach (NavigateBarButton nvb in this.List)
                if (nvb.IsDisplayed && nvb.Visible)
                    count++;

            return count;
        }

        /// <summary>
        /// Search button using key value
        /// </summary>
        /// <param name="tKey">NavigateBarButton key value</param>
        /// <returns></returns>
        public NavigateBarButton FindByKey(string tKey)
        {
            NavigateBarButton retButton = null;

            if (string.IsNullOrEmpty(tKey))
                return retButton;

            foreach (NavigateBarButton nvb in this.List)
            {
                if (!string.IsNullOrEmpty(nvb.Key) && nvb.Key.Equals(tKey))
                {
                    retButton = nvb;
                    break;
                }
            }

            return retButton;
        }

        /// <summary>
        /// Set new position in collection
        /// </summary>
        /// <param name="tButton">NavigateBarButton object</param>
        /// <param name="tNewIndex">New index in collection</param>
        public void SetChildIndex(NavigateBarButton tButton, int tNewIndex)
        {

            if (tButton == null)
                return;

            int oldIndex = this.List.IndexOf(tButton);

            // Eğer yeri daha önceli bir konuma alınmışsa
            if (oldIndex > tNewIndex)
            {
                for (int i = oldIndex; i >= (tNewIndex + 1); i--)
                    this.List[i] = this.List[i - 1];
            }
            else if (oldIndex < tNewIndex) // Eğer bulunduğu posizyondan sonraki bir posizyona alınmışsa
            {
                for (int i = oldIndex + 1; i <= tNewIndex; i++)
                    this.List[i - 1] = this.List[i];
            }

            this.List[tNewIndex] = tButton;

        }

        #endregion

    }
}
