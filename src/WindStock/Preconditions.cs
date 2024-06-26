﻿using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace WindStock
{
    internal static class Preconditions
    {
        private const string ArgumentIsEmpty = "The string cannot be empty.";
        private const string CollectionArgumentIsEmpty = "The collection must contain at least one element.";
        private const string GuidArgumentIsEmpty = "The GUID cannot be empty.";
        private const string CollectionArgumentHasNullElement = "The collection must not contain any null element.";
        private const string FileNotFound = "The file does not exist.";
        private const string DirectoryNotFound = "Directory not found at: {0}";
        private const string NumberNotPositive = "The number must be positive.";
        private const string NumberNotNegative = "The number must be negative.";
        private const string NumberNotPositiveOrZero = "The number must be positive or equals zero.";
        private const string NotTrue = "The predicate is false.";

        /// <summary>
        ///     Throws an <see cref="ArgumentNullException"/> if <paramref name="reference"/> is null.
        /// </summary>
        public static T NotNull<T>([NotNull] T reference, [CallerArgumentExpression("reference")] string? paramName = null)
        {
            if (reference is null)
            {
                ThrowArgumentNullException(paramName);
            }

            return reference;
        }

        /// <summary>
        ///     Ensures that a string is not empty, but can be null.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static string? NullableButNotEmpty(string? text, [CallerArgumentExpression("text")] string? paramName = null)
        {
            if (text is null)
            {
                return null;
            }

            if (text.Trim().Length == 0)
            {
                ThrowArgumentException(ArgumentIsEmpty, paramName);
            }

            return text;
        }

        /// <summary>
        ///     Ensures a string passed as parameter is neither null or empty.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static string NotNullOrEmpty([NotNull] string? text, [CallerArgumentExpression("text")] string? paramName = null)
        {
            if (text is null)
            {
                ThrowArgumentNullException(paramName);
            }
            else if (text.Trim().Length == 0)
            {
                ThrowArgumentException(ArgumentIsEmpty, paramName);
            }

            return text;
        }

        /// <summary>
        ///     Ensures that an enumerable contains at least one element.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static T NotNullOrEmpty<T>([NotNull] T? enumerable, [CallerArgumentExpression("enumerable")] string? paramName = null) where T : IEnumerable
        {
            NotNull(enumerable, paramName);

            if (!enumerable.GetEnumerator().MoveNext())
            {
                ThrowArgumentException(CollectionArgumentIsEmpty, paramName);
            }

            return enumerable;
        }

        /// <summary>
        ///     Ensures that an enumerable is not null and does not contain a null element.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static T HasNoNulls<T>([NotNull] T? enumerable, [CallerArgumentExpression("enumerable")] string? paramName = null) where T : IEnumerable
        {
            NotNull(enumerable, paramName);


            foreach (var item in enumerable)
            {
                if (item is null)
                {
                    ThrowArgumentException(CollectionArgumentHasNullElement, paramName);
                }
            }

            return enumerable;
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentException"/> if the GUID is null.
        /// </summary>
        public static Guid NotEmpty(Guid guid, [CallerArgumentExpression("guid")] string? paramName = null)
        {
            if (guid == Guid.Empty)
            {
                ThrowArgumentException(GuidArgumentIsEmpty, paramName);
            }

            return guid;
        }

        /// <summary>
        ///     Ensures that the specified file exists and that the caller has the required permissions.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static string FileExists([NotNull] string? path, [CallerArgumentExpression("path")] string? paramName = null)
        {
            NotNull(path, paramName);

            if (!File.Exists(path))
            {
                ThrowArgumentException(FileNotFound, paramName, new FileNotFoundException(FileNotFound, path));
            }

            return path;
        }

        /// <summary>
        ///     Ensures the given path refers to an existing directory on disk. 
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static string DirectoryExists([NotNull] string? path, [CallerArgumentExpression("path")] string? paramName = null)
        {
            NotNull(path, paramName);

            if (!Directory.Exists(path))
            {
                ThrowArgumentException(string.Format(DirectoryNotFound, path), paramName, new DirectoryNotFoundException(path));
            }

            return path;
        }

        /// <summary>
        ///     Ensures that the specified number is greater than zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T Positive<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var minimumValue = default(T);
            var compare = value.CompareTo(minimumValue);
            if (compare <= 0)
            {
                ThrowArgumentOutOfRangeException(paramName, value, NumberNotPositive);
            }

            return value;
        }

        /// <summary>
        ///     Ensures that the specified number is greater than zero.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T Positive<T>([NotNull] T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            NotNull(value, paramName);

            return Positive(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures that the specified number is greater than zero or null.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T? PositiveOrNull<T>(T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (value is null)
            {
                return null;
            }

            return Positive(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures that the specified number is less than zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T Negative<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var minimumValue = default(T);
            var compare = value.CompareTo(minimumValue);
            if (compare >= 0)
            {
                ThrowArgumentOutOfRangeException(paramName, value, NumberNotNegative);
            }

            return value;
        }

        /// <summary>
        ///     Ensures that the specified number is less than zero.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T Negative<T>(T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            NotNull(value, paramName);

            return Negative(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures that the specified number is less than zero or null.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T? NegativeOrNull<T>(T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (value is null)
            {
                return null;
            }

            return Negative(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures that the specified number is not negative. Means positive or equals zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T NotNegative<T>(T value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            var minimumValue = default(T);
            var compare = value.CompareTo(minimumValue);
            if (compare < 0)
            {
                ThrowArgumentOutOfRangeException(paramName, value, NumberNotPositiveOrZero);
            }

            return value;
        }

        /// <summary>
        ///     Ensures that the specified number is not negative. Means positive or equals zero.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T NotNegative<T>(T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            NotNull(value, paramName);

            return NotNegative(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures that the specified number is not negative or null.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static T? NotNegativeOrNull<T>(T? value, [CallerArgumentExpression("value")] string? paramName = null) where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
        {
            if (value is null)
            {
                return null;
            }

            return NotNegative(value.Value, paramName);
        }

        /// <summary>
        ///     Ensures a specific predicate is true.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static void True(Func<bool>? predicate, [CallerArgumentExpression("predicate")] string? paramName = null)
        {
            NotNull(predicate, paramName);

            if (!predicate())
            {
                ThrowArgumentException(NotTrue, paramName);
            }
        }

        /// <summary>
        ///     Ensures a specific predicate is true.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public static T? True<T>(Func<bool>? predicate, T? returnValue, [CallerArgumentExpression("predicate")] string? paramName = null)
        {
            True(predicate, paramName);

            return returnValue;
        }

        [DoesNotReturn]
        private static void ThrowArgumentNullException(string? paramName) => throw new ArgumentNullException(paramName);

        [DoesNotReturn]
        private static void ThrowArgumentOutOfRangeException(string? paramName, object? actualValue, string? message)
            => throw new ArgumentOutOfRangeException(paramName, actualValue, message);

        [DoesNotReturn]
        private static void ThrowArgumentException(string? message, string? paramName, Exception? innerException = null)
            => throw new ArgumentException(message, paramName, innerException);
    }
}
