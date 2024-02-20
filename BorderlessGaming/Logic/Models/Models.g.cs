namespace BorderlessGaming.Logic.Models {

  /// <summary>A static class which contains schema defined constants.</summary>
  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  public static class BopConstants {
    public const string DefaultCulture = "en-US";
  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Struct)]
  public partial class ProcessRectangle : global::Bebop.Runtime.BaseBebopRecord, global::System.IEquatable<ProcessRectangle> {
    /// <inheritdoc />
    public sealed override int MaxByteCount => GetMaxByteCount();
    /// <inheritdoc />
    public sealed override int ByteCount => GetByteCount();
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int X { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int Y { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int Width { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int Height { get; set; }

    /// <summary>
    /// </summary>
    public ProcessRectangle() : base() { }
    /// <summary>
    /// </summary>
    /// <param name="x">
    /// </param>
    /// <param name="y">
    /// </param>
    /// <param name="width">
    /// </param>
    /// <param name="height">
    /// </param>
    public ProcessRectangle([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int x, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int y, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int width, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int height) => (X, Y, Width, Height) = (x, y, width, height);
    public ProcessRectangle([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] ProcessRectangle original) => (X, Y, Width, Height) = (original.X, original.Y, original.Width, original.Height);
    public void Deconstruct([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int x, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int y, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int width, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int height) => (x, y, width, height) = (X, Y, Width, Height);

    /// <summary>Calculates the maximum number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetMaxByteCount() {
      int byteCount = 0;
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      return byteCount;
    }


    /// <summary>Calculates the number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetByteCount() {
      int byteCount = 0;
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      return byteCount;
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.ProcessRectangle record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.ProcessRectangle record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.ProcessRectangle record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.ProcessRectangle record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override int EncodeIntoBuffer(byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(this, ref writer);
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static int EncodeIntoBuffer(global::BorderlessGaming.Logic.Models.ProcessRectangle record, byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(record, ref writer);
    }

    #region Static Decode Methods
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.ProcessRectangle Decode(byte[] record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.ProcessRectangle Decode(global::System.ReadOnlySpan<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.ProcessRectangle Decode(global::System.ReadOnlyMemory<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.ProcessRectangle Decode(global::System.ArraySegment<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.ProcessRectangle Decode(global::System.Collections.Immutable.ImmutableArray<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    #endregion
    #region Internal Use
    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static int __EncodeInto(global::BorderlessGaming.Logic.Models.ProcessRectangle record, ref global::Bebop.Runtime.BebopWriter writer) {
      var before = writer.Length;
      writer.WriteInt32(record.X);
      writer.WriteInt32(record.Y);
      writer.WriteInt32(record.Width);
      writer.WriteInt32(record.Height);
      var after = writer.Length;
      return after - before;
    }


    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static global::BorderlessGaming.Logic.Models.ProcessRectangle __DecodeFrom(ref global::Bebop.Runtime.BebopReader reader) {

      int field0;
      field0 = reader.ReadInt32();
      int field1;
      field1 = reader.ReadInt32();
      int field2;
      field2 = reader.ReadInt32();
      int field3;
      field3 = reader.ReadInt32();
      return new global::BorderlessGaming.Logic.Models.ProcessRectangle {
        X = field0,
        Y = field1,
        Width = field2,
        Height = field3,
      };
    }

    #endregion
    #region Equality
    public bool Equals(global::BorderlessGaming.Logic.Models.ProcessRectangle other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj is not global::BorderlessGaming.Logic.Models.ProcessRectangle baseType) {
        return false;
      }
      return Equals(baseType);
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= X.GetHashCode();
      hash ^= Y.GetHashCode();
      hash ^= Width.GetHashCode();
      hash ^= Height.GetHashCode();
      return hash;
    }

    public static bool operator ==(global::BorderlessGaming.Logic.Models.ProcessRectangle left, global::BorderlessGaming.Logic.Models.ProcessRectangle right) => Equals(left, right);
    public static bool operator !=(global::BorderlessGaming.Logic.Models.ProcessRectangle left, global::BorderlessGaming.Logic.Models.ProcessRectangle  right) => !Equals(left, right);
    #endregion

  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Message)]
  public partial class AppSettings : global::Bebop.Runtime.BaseBebopRecord, global::System.IEquatable<AppSettings> {
    #nullable enable
    /// <inheritdoc />
    public sealed override int MaxByteCount => GetMaxByteCount();
    /// <inheritdoc />
    public sealed override int ByteCount => GetByteCount();
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? SlowWindowDetection { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? ViewAllProcessDetails { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? RunOnStartup { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? UseGlobalHotkey { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? UseMouseLockHotKey { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? UseMouseHideHotKey { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? StartMinimized { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? HideBalloonTips { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? CloseToTray { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? CheckForUpdates { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public bool? DisableSteamIntegration { get; set; }
    [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull]
    public string? Culture { get; set; }

    /// <summary>
    /// </summary>
    public AppSettings() : base() { }
    /// <summary>
    /// </summary>
    /// <param name="slowWindowDetection">
    /// </param>
    /// <param name="viewAllProcessDetails">
    /// </param>
    /// <param name="runOnStartup">
    /// </param>
    /// <param name="useGlobalHotkey">
    /// </param>
    /// <param name="useMouseLockHotKey">
    /// </param>
    /// <param name="useMouseHideHotKey">
    /// </param>
    /// <param name="startMinimized">
    /// </param>
    /// <param name="hideBalloonTips">
    /// </param>
    /// <param name="closeToTray">
    /// </param>
    /// <param name="checkForUpdates">
    /// </param>
    /// <param name="disableSteamIntegration">
    /// </param>
    /// <param name="culture">
    /// </param>
    public AppSettings([global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? slowWindowDetection, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? viewAllProcessDetails, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? runOnStartup, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? useGlobalHotkey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? useMouseLockHotKey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? useMouseHideHotKey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? startMinimized, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? hideBalloonTips, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? closeToTray, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? checkForUpdates, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] bool? disableSteamIntegration, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] string? culture) => (SlowWindowDetection, ViewAllProcessDetails, RunOnStartup, UseGlobalHotkey, UseMouseLockHotKey, UseMouseHideHotKey, StartMinimized, HideBalloonTips, CloseToTray, CheckForUpdates, DisableSteamIntegration, Culture) = (slowWindowDetection, viewAllProcessDetails, runOnStartup, useGlobalHotkey, useMouseLockHotKey, useMouseHideHotKey, startMinimized, hideBalloonTips, closeToTray, checkForUpdates, disableSteamIntegration, culture);
    public AppSettings([global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] AppSettings? original) => (SlowWindowDetection, ViewAllProcessDetails, RunOnStartup, UseGlobalHotkey, UseMouseLockHotKey, UseMouseHideHotKey, StartMinimized, HideBalloonTips, CloseToTray, CheckForUpdates, DisableSteamIntegration, Culture) = (original?.SlowWindowDetection, original?.ViewAllProcessDetails, original?.RunOnStartup, original?.UseGlobalHotkey, original?.UseMouseLockHotKey, original?.UseMouseHideHotKey, original?.StartMinimized, original?.HideBalloonTips, original?.CloseToTray, original?.CheckForUpdates, original?.DisableSteamIntegration, original?.Culture);
    public void Deconstruct([global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? slowWindowDetection, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? viewAllProcessDetails, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? runOnStartup, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? useGlobalHotkey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? useMouseLockHotKey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? useMouseHideHotKey, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? startMinimized, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? hideBalloonTips, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? closeToTray, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? checkForUpdates, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out bool? disableSteamIntegration, [global::System.Diagnostics.CodeAnalysis.MaybeNull, global::System.Diagnostics.CodeAnalysis.AllowNull] out string? culture) => (slowWindowDetection, viewAllProcessDetails, runOnStartup, useGlobalHotkey, useMouseLockHotKey, useMouseHideHotKey, startMinimized, hideBalloonTips, closeToTray, checkForUpdates, disableSteamIntegration, culture) = (SlowWindowDetection, ViewAllProcessDetails, RunOnStartup, UseGlobalHotkey, UseMouseLockHotKey, UseMouseHideHotKey, StartMinimized, HideBalloonTips, CloseToTray, CheckForUpdates, DisableSteamIntegration, Culture);

    /// <summary>Calculates the maximum number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetMaxByteCount() {
      int byteCount = 0;
      byteCount += 5;
      if (SlowWindowDetection is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (ViewAllProcessDetails is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (RunOnStartup is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseGlobalHotkey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseMouseLockHotKey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseMouseHideHotKey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (StartMinimized is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (HideBalloonTips is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (CloseToTray is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (CheckForUpdates is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (DisableSteamIntegration is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (Culture is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(Culture.Length);
      }
      return byteCount;
    }


    /// <summary>Calculates the number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetByteCount() {
      int byteCount = 0;
      byteCount += 5;
      if (SlowWindowDetection is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (ViewAllProcessDetails is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (RunOnStartup is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseGlobalHotkey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseMouseLockHotKey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (UseMouseHideHotKey is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (StartMinimized is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (HideBalloonTips is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (CloseToTray is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (CheckForUpdates is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (DisableSteamIntegration is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(bool);
      }
      if (Culture is not null) {
        byteCount += sizeof(byte);
        byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(Culture);
      }
      return byteCount;
    }

    #nullable disable

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.AppSettings record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.AppSettings record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.AppSettings record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.AppSettings record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override int EncodeIntoBuffer(byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(this, ref writer);
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static int EncodeIntoBuffer(global::BorderlessGaming.Logic.Models.AppSettings record, byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(record, ref writer);
    }

    #region Static Decode Methods
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.AppSettings Decode(byte[] record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.AppSettings Decode(global::System.ReadOnlySpan<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.AppSettings Decode(global::System.ReadOnlyMemory<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.AppSettings Decode(global::System.ArraySegment<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.AppSettings Decode(global::System.Collections.Immutable.ImmutableArray<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    #endregion
    #region Internal Use
    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static int __EncodeInto(global::BorderlessGaming.Logic.Models.AppSettings record, ref global::Bebop.Runtime.BebopWriter writer) {
      var before = writer.Length;
      var pos = writer.ReserveRecordLength();
      var start = writer.Length;

      if (record.SlowWindowDetection is not null) {
        writer.WriteByte(1);
        writer.WriteByte(record.SlowWindowDetection.Value);
      }

      if (record.ViewAllProcessDetails is not null) {
        writer.WriteByte(2);
        writer.WriteByte(record.ViewAllProcessDetails.Value);
      }

      if (record.RunOnStartup is not null) {
        writer.WriteByte(3);
        writer.WriteByte(record.RunOnStartup.Value);
      }

      if (record.UseGlobalHotkey is not null) {
        writer.WriteByte(4);
        writer.WriteByte(record.UseGlobalHotkey.Value);
      }

      if (record.UseMouseLockHotKey is not null) {
        writer.WriteByte(5);
        writer.WriteByte(record.UseMouseLockHotKey.Value);
      }

      if (record.UseMouseHideHotKey is not null) {
        writer.WriteByte(6);
        writer.WriteByte(record.UseMouseHideHotKey.Value);
      }

      if (record.StartMinimized is not null) {
        writer.WriteByte(7);
        writer.WriteByte(record.StartMinimized.Value);
      }

      if (record.HideBalloonTips is not null) {
        writer.WriteByte(8);
        writer.WriteByte(record.HideBalloonTips.Value);
      }

      if (record.CloseToTray is not null) {
        writer.WriteByte(9);
        writer.WriteByte(record.CloseToTray.Value);
      }

      if (record.CheckForUpdates is not null) {
        writer.WriteByte(10);
        writer.WriteByte(record.CheckForUpdates.Value);
      }

      if (record.DisableSteamIntegration is not null) {
        writer.WriteByte(11);
        writer.WriteByte(record.DisableSteamIntegration.Value);
      }

      if (record.Culture is not null) {
        writer.WriteByte(12);
        writer.WriteString(record.Culture);
      }
      writer.WriteByte(0);
      var end = writer.Length;
      writer.FillRecordLength(pos, unchecked((uint) unchecked(end - start)));
      var after = writer.Length;
      return after - before;
    }


    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static global::BorderlessGaming.Logic.Models.AppSettings __DecodeFrom(ref global::Bebop.Runtime.BebopReader reader) {

      var record = new global::BorderlessGaming.Logic.Models.AppSettings();
      var length = reader.ReadRecordLength();
      var end = unchecked((int) (reader.Position + length));
      while (true) {
        switch (reader.ReadByte()) {
          case 0:
            return record;
          case 1:
            record.SlowWindowDetection = reader.ReadByte() != 0;
            break;
          case 2:
            record.ViewAllProcessDetails = reader.ReadByte() != 0;
            break;
          case 3:
            record.RunOnStartup = reader.ReadByte() != 0;
            break;
          case 4:
            record.UseGlobalHotkey = reader.ReadByte() != 0;
            break;
          case 5:
            record.UseMouseLockHotKey = reader.ReadByte() != 0;
            break;
          case 6:
            record.UseMouseHideHotKey = reader.ReadByte() != 0;
            break;
          case 7:
            record.StartMinimized = reader.ReadByte() != 0;
            break;
          case 8:
            record.HideBalloonTips = reader.ReadByte() != 0;
            break;
          case 9:
            record.CloseToTray = reader.ReadByte() != 0;
            break;
          case 10:
            record.CheckForUpdates = reader.ReadByte() != 0;
            break;
          case 11:
            record.DisableSteamIntegration = reader.ReadByte() != 0;
            break;
          case 12:
            record.Culture = reader.ReadString();
            break;
          default:
            reader.Position = end;
            return record;
        }
      }
    }

    #endregion
    #region Equality
    public bool Equals(global::BorderlessGaming.Logic.Models.AppSettings other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return SlowWindowDetection == other.SlowWindowDetection && ViewAllProcessDetails == other.ViewAllProcessDetails && RunOnStartup == other.RunOnStartup && UseGlobalHotkey == other.UseGlobalHotkey && UseMouseLockHotKey == other.UseMouseLockHotKey && UseMouseHideHotKey == other.UseMouseHideHotKey && StartMinimized == other.StartMinimized && HideBalloonTips == other.HideBalloonTips && CloseToTray == other.CloseToTray && CheckForUpdates == other.CheckForUpdates && DisableSteamIntegration == other.DisableSteamIntegration && Culture == other.Culture;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj is not global::BorderlessGaming.Logic.Models.AppSettings baseType) {
        return false;
      }
      return Equals(baseType);
    }

    public override int GetHashCode() {
      int hash = 1;
      if (SlowWindowDetection is not null) hash ^= SlowWindowDetection.Value.GetHashCode();
      if (ViewAllProcessDetails is not null) hash ^= ViewAllProcessDetails.Value.GetHashCode();
      if (RunOnStartup is not null) hash ^= RunOnStartup.Value.GetHashCode();
      if (UseGlobalHotkey is not null) hash ^= UseGlobalHotkey.Value.GetHashCode();
      if (UseMouseLockHotKey is not null) hash ^= UseMouseLockHotKey.Value.GetHashCode();
      if (UseMouseHideHotKey is not null) hash ^= UseMouseHideHotKey.Value.GetHashCode();
      if (StartMinimized is not null) hash ^= StartMinimized.Value.GetHashCode();
      if (HideBalloonTips is not null) hash ^= HideBalloonTips.Value.GetHashCode();
      if (CloseToTray is not null) hash ^= CloseToTray.Value.GetHashCode();
      if (CheckForUpdates is not null) hash ^= CheckForUpdates.Value.GetHashCode();
      if (DisableSteamIntegration is not null) hash ^= DisableSteamIntegration.Value.GetHashCode();
      if (Culture is not null) hash ^= Culture.GetHashCode();
      return hash;
    }

    public static bool operator ==(global::BorderlessGaming.Logic.Models.AppSettings left, global::BorderlessGaming.Logic.Models.AppSettings right) => Equals(left, right);
    public static bool operator !=(global::BorderlessGaming.Logic.Models.AppSettings left, global::BorderlessGaming.Logic.Models.AppSettings  right) => !Equals(left, right);
    #endregion

  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Struct, true)]
  public partial class RuntimeException : global::Bebop.Runtime.BaseBebopRecord, global::System.IEquatable<RuntimeException> {
    /// <inheritdoc />
    public sealed override int MaxByteCount => GetMaxByteCount();
    /// <inheritdoc />
    public sealed override int ByteCount => GetByteCount();
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string Reason { get; init; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string InnerReason { get; init; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string Type { get; init; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string StackTrace { get; init; }

    /// <summary>
    /// </summary>
    public RuntimeException() : base() { }
    /// <summary>
    /// </summary>
    /// <param name="reason">
    /// </param>
    /// <param name="innerReason">
    /// </param>
    /// <param name="type">
    /// </param>
    /// <param name="stackTrace">
    /// </param>
    public RuntimeException([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string reason, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string innerReason, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string type, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string stackTrace) => (Reason, InnerReason, Type, StackTrace) = (reason, innerReason, type, stackTrace);
    public RuntimeException([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] RuntimeException original) => (Reason, InnerReason, Type, StackTrace) = (original.Reason, original.InnerReason, original.Type, original.StackTrace);
    public void Deconstruct([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string reason, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string innerReason, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string type, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string stackTrace) => (reason, innerReason, type, stackTrace) = (Reason, InnerReason, Type, StackTrace);

    /// <summary>Calculates the maximum number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetMaxByteCount() {
      int byteCount = 0;
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(Reason.Length);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(InnerReason.Length);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(Type.Length);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(StackTrace.Length);
      return byteCount;
    }


    /// <summary>Calculates the number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetByteCount() {
      int byteCount = 0;
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(Reason);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(InnerReason);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(Type);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(StackTrace);
      return byteCount;
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.RuntimeException record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.RuntimeException record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.RuntimeException record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.RuntimeException record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override int EncodeIntoBuffer(byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(this, ref writer);
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static int EncodeIntoBuffer(global::BorderlessGaming.Logic.Models.RuntimeException record, byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(record, ref writer);
    }

    #region Static Decode Methods
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.RuntimeException Decode(byte[] record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.RuntimeException Decode(global::System.ReadOnlySpan<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.RuntimeException Decode(global::System.ReadOnlyMemory<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.RuntimeException Decode(global::System.ArraySegment<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.RuntimeException Decode(global::System.Collections.Immutable.ImmutableArray<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    #endregion
    #region Internal Use
    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static int __EncodeInto(global::BorderlessGaming.Logic.Models.RuntimeException record, ref global::Bebop.Runtime.BebopWriter writer) {
      var before = writer.Length;
      writer.WriteString(record.Reason);
      writer.WriteString(record.InnerReason);
      writer.WriteString(record.Type);
      writer.WriteString(record.StackTrace);
      var after = writer.Length;
      return after - before;
    }


    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static global::BorderlessGaming.Logic.Models.RuntimeException __DecodeFrom(ref global::Bebop.Runtime.BebopReader reader) {

      string field0;
      field0 = reader.ReadString();
      string field1;
      field1 = reader.ReadString();
      string field2;
      field2 = reader.ReadString();
      string field3;
      field3 = reader.ReadString();
      return new global::BorderlessGaming.Logic.Models.RuntimeException {
        Reason = field0,
        InnerReason = field1,
        Type = field2,
        StackTrace = field3,
      };
    }

    #endregion
    #region Equality
    public bool Equals(global::BorderlessGaming.Logic.Models.RuntimeException other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return Reason == other.Reason && InnerReason == other.InnerReason && Type == other.Type && StackTrace == other.StackTrace;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj is not global::BorderlessGaming.Logic.Models.RuntimeException baseType) {
        return false;
      }
      return Equals(baseType);
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= Reason.GetHashCode();
      hash ^= InnerReason.GetHashCode();
      hash ^= Type.GetHashCode();
      hash ^= StackTrace.GetHashCode();
      return hash;
    }

    public static bool operator ==(global::BorderlessGaming.Logic.Models.RuntimeException left, global::BorderlessGaming.Logic.Models.RuntimeException right) => Equals(left, right);
    public static bool operator !=(global::BorderlessGaming.Logic.Models.RuntimeException left, global::BorderlessGaming.Logic.Models.RuntimeException  right) => !Equals(left, right);
    #endregion

  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Struct)]
  public partial class UserPreferences : global::Bebop.Runtime.BaseBebopRecord, global::System.IEquatable<UserPreferences> {
    /// <inheritdoc />
    public sealed override int MaxByteCount => GetMaxByteCount();
    /// <inheritdoc />
    public sealed override int ByteCount => GetByteCount();
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public global::BorderlessGaming.Logic.Models.Favorite[] Favorites { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string[] HiddenProcesses { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public global::BorderlessGaming.Logic.Models.AppSettings Settings { get; set; }

    /// <summary>
    /// </summary>
    public UserPreferences() : base() { }
    /// <summary>
    /// </summary>
    /// <param name="favorites">
    /// </param>
    /// <param name="hiddenProcesses">
    /// </param>
    /// <param name="settings">
    /// </param>
    public UserPreferences([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] global::BorderlessGaming.Logic.Models.Favorite[] favorites, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string[] hiddenProcesses, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] global::BorderlessGaming.Logic.Models.AppSettings settings) => (Favorites, HiddenProcesses, Settings) = (favorites, hiddenProcesses, settings);
    public UserPreferences([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] UserPreferences original) => (Favorites, HiddenProcesses, Settings) = (original.Favorites, original.HiddenProcesses, original.Settings);
    public void Deconstruct([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out global::BorderlessGaming.Logic.Models.Favorite[] favorites, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string[] hiddenProcesses, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out global::BorderlessGaming.Logic.Models.AppSettings settings) => (favorites, hiddenProcesses, settings) = (Favorites, HiddenProcesses, Settings);

    /// <summary>Calculates the maximum number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetMaxByteCount() {
      int byteCount = 0;
      {
        var length0 = unchecked((uint)Favorites.Length);
        byteCount += sizeof(uint);
        for (var i0 = 0; i0 < length0; i0++) {
          byteCount += Favorites[i0].MaxByteCount;
        }
      }
      {
        var length0 = unchecked((uint)HiddenProcesses.Length);
        byteCount += sizeof(uint);
        for (var i0 = 0; i0 < length0; i0++) {
          byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(HiddenProcesses[i0].Length);
        }
      }
      byteCount += Settings.MaxByteCount;
      return byteCount;
    }


    /// <summary>Calculates the number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetByteCount() {
      int byteCount = 0;
      {
        var length0 = unchecked((uint)Favorites.Length);
        byteCount += sizeof(uint);
        for (var i0 = 0; i0 < length0; i0++) {
          byteCount += Favorites[i0].ByteCount;
        }
      }
      {
        var length0 = unchecked((uint)HiddenProcesses.Length);
        byteCount += sizeof(uint);
        for (var i0 = 0; i0 < length0; i0++) {
          byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(HiddenProcesses[i0]);
        }
      }
      byteCount += Settings.ByteCount;
      return byteCount;
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.UserPreferences record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.UserPreferences record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.UserPreferences record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.UserPreferences record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override int EncodeIntoBuffer(byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(this, ref writer);
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static int EncodeIntoBuffer(global::BorderlessGaming.Logic.Models.UserPreferences record, byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(record, ref writer);
    }

    #region Static Decode Methods
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.UserPreferences Decode(byte[] record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.UserPreferences Decode(global::System.ReadOnlySpan<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.UserPreferences Decode(global::System.ReadOnlyMemory<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.UserPreferences Decode(global::System.ArraySegment<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.UserPreferences Decode(global::System.Collections.Immutable.ImmutableArray<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    #endregion
    #region Internal Use
    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static int __EncodeInto(global::BorderlessGaming.Logic.Models.UserPreferences record, ref global::Bebop.Runtime.BebopWriter writer) {
      var before = writer.Length;
      {
        var length0 = unchecked((uint)record.Favorites.Length);
        writer.WriteUInt32(length0);
        for (var i0 = 0; i0 < length0; i0++) {
          global::BorderlessGaming.Logic.Models.Favorite.__EncodeInto(record.Favorites[i0], ref writer);
        }
      }
      {
        var length0 = unchecked((uint)record.HiddenProcesses.Length);
        writer.WriteUInt32(length0);
        for (var i0 = 0; i0 < length0; i0++) {
          writer.WriteString(record.HiddenProcesses[i0]);
        }
      }
      global::BorderlessGaming.Logic.Models.AppSettings.__EncodeInto(record.Settings, ref writer);
      var after = writer.Length;
      return after - before;
    }


    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static global::BorderlessGaming.Logic.Models.UserPreferences __DecodeFrom(ref global::Bebop.Runtime.BebopReader reader) {

      global::BorderlessGaming.Logic.Models.Favorite[] field0;
      {
        var length0 = unchecked((int)reader.ReadUInt32());
        field0 = new global::BorderlessGaming.Logic.Models.Favorite[length0];
        for (var i0 = 0; i0 < length0; i0++) {
          global::BorderlessGaming.Logic.Models.Favorite x0;
          x0 = global::BorderlessGaming.Logic.Models.Favorite.__DecodeFrom(ref reader);
          field0[i0] = x0;
        }
      }
      string[] field1;
      {
        var length0 = unchecked((int)reader.ReadUInt32());
        field1 = new string[length0];
        for (var i0 = 0; i0 < length0; i0++) {
          string x0;
          x0 = reader.ReadString();
          field1[i0] = x0;
        }
      }
      global::BorderlessGaming.Logic.Models.AppSettings field2;
      field2 = global::BorderlessGaming.Logic.Models.AppSettings.__DecodeFrom(ref reader);
      return new global::BorderlessGaming.Logic.Models.UserPreferences {
        Favorites = field0,
        HiddenProcesses = field1,
        Settings = field2,
      };
    }

    #endregion
    #region Equality
    public bool Equals(global::BorderlessGaming.Logic.Models.UserPreferences other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return (Favorites is null ? other.Favorites is null : other.Favorites is not null && global::System.Linq.Enumerable.SequenceEqual(Favorites, other.Favorites)) && (HiddenProcesses is null ? other.HiddenProcesses is null : other.HiddenProcesses is not null && global::System.Linq.Enumerable.SequenceEqual(HiddenProcesses, other.HiddenProcesses)) && Settings == other.Settings;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj is not global::BorderlessGaming.Logic.Models.UserPreferences baseType) {
        return false;
      }
      return Equals(baseType);
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= Favorites.GetHashCode();
      hash ^= HiddenProcesses.GetHashCode();
      hash ^= Settings.GetHashCode();
      return hash;
    }

    public static bool operator ==(global::BorderlessGaming.Logic.Models.UserPreferences left, global::BorderlessGaming.Logic.Models.UserPreferences right) => Equals(left, right);
    public static bool operator !=(global::BorderlessGaming.Logic.Models.UserPreferences left, global::BorderlessGaming.Logic.Models.UserPreferences  right) => !Equals(left, right);
    #endregion

  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Enum)]
  public enum FavoriteSize : byte {
    Invalid = 0,
    FullScreen = 1,
    SpecificSize = 2,
    NoChange = 3
  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Enum)]
  public enum FavoriteType : byte {
    Invalid = 0,
    Process = 1,
    Title = 2,
    Regex = 3
  }

  [global::System.CodeDom.Compiler.GeneratedCode("bebopc", "2.8.7")]
  [global::Bebop.Attributes.BebopRecord(global::Bebop.Runtime.BebopKind.Struct)]
  public partial class Favorite : global::Bebop.Runtime.BaseBebopRecord, global::System.IEquatable<Favorite> {
    /// <inheritdoc />
    public sealed override int MaxByteCount => GetMaxByteCount();
    /// <inheritdoc />
    public sealed override int ByteCount => GetByteCount();
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public global::BorderlessGaming.Logic.Models.FavoriteType Type { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public global::BorderlessGaming.Logic.Models.FavoriteSize Size { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public string SearchText { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public global::BorderlessGaming.Logic.Models.ProcessRectangle Screen { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int OffsetLeft { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int OffsetTop { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int OffsetRight { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int OffsetBottom { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool ShouldMaximize { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int PositionX { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int PositionY { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int PositionWidth { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public int PositionHeight { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool RemoveMenus { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool TopMost { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool HideWindowsTaskbar { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool HideMouseCursor { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool DelayBorderless { get; set; }
    [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull]
    public bool MuteInBackground { get; set; }

    /// <summary>
    /// </summary>
    public Favorite() : base() { }
    /// <summary>
    /// </summary>
    /// <param name="type">
    /// </param>
    /// <param name="size">
    /// </param>
    /// <param name="searchText">
    /// </param>
    /// <param name="screen">
    /// </param>
    /// <param name="offsetLeft">
    /// </param>
    /// <param name="offsetTop">
    /// </param>
    /// <param name="offsetRight">
    /// </param>
    /// <param name="offsetBottom">
    /// </param>
    /// <param name="shouldMaximize">
    /// </param>
    /// <param name="positionX">
    /// </param>
    /// <param name="positionY">
    /// </param>
    /// <param name="positionWidth">
    /// </param>
    /// <param name="positionHeight">
    /// </param>
    /// <param name="removeMenus">
    /// </param>
    /// <param name="topMost">
    /// </param>
    /// <param name="hideWindowsTaskbar">
    /// </param>
    /// <param name="hideMouseCursor">
    /// </param>
    /// <param name="delayBorderless">
    /// </param>
    /// <param name="muteInBackground">
    /// </param>
    public Favorite([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] global::BorderlessGaming.Logic.Models.FavoriteType type, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] global::BorderlessGaming.Logic.Models.FavoriteSize size, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] string searchText, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] global::BorderlessGaming.Logic.Models.ProcessRectangle screen, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int offsetLeft, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int offsetTop, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int offsetRight, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int offsetBottom, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool shouldMaximize, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int positionX, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int positionY, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int positionWidth, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] int positionHeight, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool removeMenus, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool topMost, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool hideWindowsTaskbar, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool hideMouseCursor, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool delayBorderless, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] bool muteInBackground) => (Type, Size, SearchText, Screen, OffsetLeft, OffsetTop, OffsetRight, OffsetBottom, ShouldMaximize, PositionX, PositionY, PositionWidth, PositionHeight, RemoveMenus, TopMost, HideWindowsTaskbar, HideMouseCursor, DelayBorderless, MuteInBackground) = (type, size, searchText, screen, offsetLeft, offsetTop, offsetRight, offsetBottom, shouldMaximize, positionX, positionY, positionWidth, positionHeight, removeMenus, topMost, hideWindowsTaskbar, hideMouseCursor, delayBorderless, muteInBackground);
    public Favorite([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] Favorite original) => (Type, Size, SearchText, Screen, OffsetLeft, OffsetTop, OffsetRight, OffsetBottom, ShouldMaximize, PositionX, PositionY, PositionWidth, PositionHeight, RemoveMenus, TopMost, HideWindowsTaskbar, HideMouseCursor, DelayBorderless, MuteInBackground) = (original.Type, original.Size, original.SearchText, original.Screen, original.OffsetLeft, original.OffsetTop, original.OffsetRight, original.OffsetBottom, original.ShouldMaximize, original.PositionX, original.PositionY, original.PositionWidth, original.PositionHeight, original.RemoveMenus, original.TopMost, original.HideWindowsTaskbar, original.HideMouseCursor, original.DelayBorderless, original.MuteInBackground);
    public void Deconstruct([global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out global::BorderlessGaming.Logic.Models.FavoriteType type, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out global::BorderlessGaming.Logic.Models.FavoriteSize size, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out string searchText, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out global::BorderlessGaming.Logic.Models.ProcessRectangle screen, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int offsetLeft, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int offsetTop, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int offsetRight, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int offsetBottom, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool shouldMaximize, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int positionX, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int positionY, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int positionWidth, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out int positionHeight, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool removeMenus, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool topMost, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool hideWindowsTaskbar, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool hideMouseCursor, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool delayBorderless, [global::System.Diagnostics.CodeAnalysis.NotNull, global::System.Diagnostics.CodeAnalysis.DisallowNull] out bool muteInBackground) => (type, size, searchText, screen, offsetLeft, offsetTop, offsetRight, offsetBottom, shouldMaximize, positionX, positionY, positionWidth, positionHeight, removeMenus, topMost, hideWindowsTaskbar, hideMouseCursor, delayBorderless, muteInBackground) = (Type, Size, SearchText, Screen, OffsetLeft, OffsetTop, OffsetRight, OffsetBottom, ShouldMaximize, PositionX, PositionY, PositionWidth, PositionHeight, RemoveMenus, TopMost, HideWindowsTaskbar, HideMouseCursor, DelayBorderless, MuteInBackground);

    /// <summary>Calculates the maximum number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetMaxByteCount() {
      int byteCount = 0;
      byteCount += sizeof(byte);
      byteCount += sizeof(byte);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetMaxByteCount(SearchText.Length);
      byteCount += Screen.MaxByteCount;
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(bool);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      return byteCount;
    }


    /// <summary>Calculates the number of bytes produced by encoding the current record.</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    private protected int GetByteCount() {
      int byteCount = 0;
      byteCount += sizeof(byte);
      byteCount += sizeof(byte);
      byteCount += sizeof(uint) + global::System.Text.Encoding.UTF8.GetByteCount(SearchText);
      byteCount += Screen.ByteCount;
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(bool);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(int);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      byteCount += sizeof(bool);
      return byteCount;
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.Favorite record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override byte[] Encode(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static byte[] Encode(global::BorderlessGaming.Logic.Models.Favorite record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably() {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.Favorite record) {
      var writer = global::Bebop.Runtime.BebopWriter.Create();
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(this, ref writer);
      return writer.ToImmutableArray();
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::System.Collections.Immutable.ImmutableArray<byte> EncodeImmutably(global::BorderlessGaming.Logic.Models.Favorite record, int initialCapacity) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(initialCapacity);
      __EncodeInto(record, ref writer);
      return writer.ToImmutableArray();
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public sealed override int EncodeIntoBuffer(byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(this, ref writer);
    }
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static int EncodeIntoBuffer(global::BorderlessGaming.Logic.Models.Favorite record, byte[] outBuffer) {
      var writer = global::Bebop.Runtime.BebopWriter.Create(outBuffer);
      return __EncodeInto(record, ref writer);
    }

    #region Static Decode Methods
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.Favorite Decode(byte[] record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.Favorite Decode(global::System.ReadOnlySpan<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.Favorite Decode(global::System.ReadOnlyMemory<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.Favorite Decode(global::System.ArraySegment<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    public static global::BorderlessGaming.Logic.Models.Favorite Decode(global::System.Collections.Immutable.ImmutableArray<byte> record) {
      var reader = global::Bebop.Runtime.BebopReader.From(record);
      return __DecodeFrom(ref reader);
    }

    #endregion
    #region Internal Use
    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static int __EncodeInto(global::BorderlessGaming.Logic.Models.Favorite record, ref global::Bebop.Runtime.BebopWriter writer) {
      var before = writer.Length;
      writer.WriteByte(System.Runtime.CompilerServices.Unsafe.As<global::BorderlessGaming.Logic.Models.FavoriteType, byte>(ref System.Runtime.CompilerServices.Unsafe.AsRef(record.Type)));
      writer.WriteByte(System.Runtime.CompilerServices.Unsafe.As<global::BorderlessGaming.Logic.Models.FavoriteSize, byte>(ref System.Runtime.CompilerServices.Unsafe.AsRef(record.Size)));
      writer.WriteString(record.SearchText);
      global::BorderlessGaming.Logic.Models.ProcessRectangle.__EncodeInto(record.Screen, ref writer);
      writer.WriteInt32(record.OffsetLeft);
      writer.WriteInt32(record.OffsetTop);
      writer.WriteInt32(record.OffsetRight);
      writer.WriteInt32(record.OffsetBottom);
      writer.WriteByte(record.ShouldMaximize);
      writer.WriteInt32(record.PositionX);
      writer.WriteInt32(record.PositionY);
      writer.WriteInt32(record.PositionWidth);
      writer.WriteInt32(record.PositionHeight);
      writer.WriteByte(record.RemoveMenus);
      writer.WriteByte(record.TopMost);
      writer.WriteByte(record.HideWindowsTaskbar);
      writer.WriteByte(record.HideMouseCursor);
      writer.WriteByte(record.DelayBorderless);
      writer.WriteByte(record.MuteInBackground);
      var after = writer.Length;
      return after - before;
    }


    /// <summary>DO NOT CALL THIS METHOD DIRECTLY!</summary>
    [global::System.Runtime.CompilerServices.MethodImpl(global::Bebop.Runtime.BebopConstants.HotPath)]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal static global::BorderlessGaming.Logic.Models.Favorite __DecodeFrom(ref global::Bebop.Runtime.BebopReader reader) {

      global::BorderlessGaming.Logic.Models.FavoriteType field0;
      field0 = System.Runtime.CompilerServices.Unsafe.As<byte, global::BorderlessGaming.Logic.Models.FavoriteType>(ref System.Runtime.CompilerServices.Unsafe.AsRef(reader.ReadByte()));
      global::BorderlessGaming.Logic.Models.FavoriteSize field1;
      field1 = System.Runtime.CompilerServices.Unsafe.As<byte, global::BorderlessGaming.Logic.Models.FavoriteSize>(ref System.Runtime.CompilerServices.Unsafe.AsRef(reader.ReadByte()));
      string field2;
      field2 = reader.ReadString();
      global::BorderlessGaming.Logic.Models.ProcessRectangle field3;
      field3 = global::BorderlessGaming.Logic.Models.ProcessRectangle.__DecodeFrom(ref reader);
      int field4;
      field4 = reader.ReadInt32();
      int field5;
      field5 = reader.ReadInt32();
      int field6;
      field6 = reader.ReadInt32();
      int field7;
      field7 = reader.ReadInt32();
      bool field8;
      field8 = reader.ReadByte() != 0;
      int field9;
      field9 = reader.ReadInt32();
      int field10;
      field10 = reader.ReadInt32();
      int field11;
      field11 = reader.ReadInt32();
      int field12;
      field12 = reader.ReadInt32();
      bool field13;
      field13 = reader.ReadByte() != 0;
      bool field14;
      field14 = reader.ReadByte() != 0;
      bool field15;
      field15 = reader.ReadByte() != 0;
      bool field16;
      field16 = reader.ReadByte() != 0;
      bool field17;
      field17 = reader.ReadByte() != 0;
      bool field18;
      field18 = reader.ReadByte() != 0;
      return new global::BorderlessGaming.Logic.Models.Favorite {
        Type = field0,
        Size = field1,
        SearchText = field2,
        Screen = field3,
        OffsetLeft = field4,
        OffsetTop = field5,
        OffsetRight = field6,
        OffsetBottom = field7,
        ShouldMaximize = field8,
        PositionX = field9,
        PositionY = field10,
        PositionWidth = field11,
        PositionHeight = field12,
        RemoveMenus = field13,
        TopMost = field14,
        HideWindowsTaskbar = field15,
        HideMouseCursor = field16,
        DelayBorderless = field17,
        MuteInBackground = field18,
      };
    }

    #endregion
    #region Equality
    public bool Equals(global::BorderlessGaming.Logic.Models.Favorite other) {
      if (ReferenceEquals(null, other)) {
        return false;
      }
      if (ReferenceEquals(this, other)) {
        return true;
      }
      return Type == other.Type && Size == other.Size && SearchText == other.SearchText && Screen == other.Screen && OffsetLeft == other.OffsetLeft && OffsetTop == other.OffsetTop && OffsetRight == other.OffsetRight && OffsetBottom == other.OffsetBottom && ShouldMaximize == other.ShouldMaximize && PositionX == other.PositionX && PositionY == other.PositionY && PositionWidth == other.PositionWidth && PositionHeight == other.PositionHeight && RemoveMenus == other.RemoveMenus && TopMost == other.TopMost && HideWindowsTaskbar == other.HideWindowsTaskbar && HideMouseCursor == other.HideMouseCursor && DelayBorderless == other.DelayBorderless && MuteInBackground == other.MuteInBackground;
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(null, obj)) {
        return false;
      }
      if (ReferenceEquals(this, obj)) {
        return true;
      }
      if (obj is not global::BorderlessGaming.Logic.Models.Favorite baseType) {
        return false;
      }
      return Equals(baseType);
    }

    public override int GetHashCode() {
      int hash = 1;
      hash ^= Type.GetHashCode();
      hash ^= Size.GetHashCode();
      hash ^= SearchText.GetHashCode();
      hash ^= Screen.GetHashCode();
      hash ^= OffsetLeft.GetHashCode();
      hash ^= OffsetTop.GetHashCode();
      hash ^= OffsetRight.GetHashCode();
      hash ^= OffsetBottom.GetHashCode();
      hash ^= ShouldMaximize.GetHashCode();
      hash ^= PositionX.GetHashCode();
      hash ^= PositionY.GetHashCode();
      hash ^= PositionWidth.GetHashCode();
      hash ^= PositionHeight.GetHashCode();
      hash ^= RemoveMenus.GetHashCode();
      hash ^= TopMost.GetHashCode();
      hash ^= HideWindowsTaskbar.GetHashCode();
      hash ^= HideMouseCursor.GetHashCode();
      hash ^= DelayBorderless.GetHashCode();
      hash ^= MuteInBackground.GetHashCode();
      return hash;
    }

    public static bool operator ==(global::BorderlessGaming.Logic.Models.Favorite left, global::BorderlessGaming.Logic.Models.Favorite right) => Equals(left, right);
    public static bool operator !=(global::BorderlessGaming.Logic.Models.Favorite left, global::BorderlessGaming.Logic.Models.Favorite  right) => !Equals(left, right);
    #endregion

  }

}
