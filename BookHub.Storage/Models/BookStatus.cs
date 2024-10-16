﻿namespace BookHub.Storage.Models;

/// <summary>
/// Статус книги.
/// </summary>
public enum BookStatus
{
    /// <summary>
    /// Черновик.
    /// </summary>
    /// <remarks>
    /// Возможно не нужно пока.
    /// </remarks>
    Draft,

    /// <summary>
    /// Опубликована.
    /// </summary>
    Published,

    /// <summary>
    /// Скрыта.
    /// </summary>
    Hiden,

    /// <summary>
    /// Удалена.
    /// </summary>
    Removed
}
