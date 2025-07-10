#pragma warning disable

using Capnp;
using Capnp.Rpc;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CapnpGen
{
    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d72a49ccb3ac4d0UL), Proxy(typeof(CMCtlPins_Proxy)), Skeleton(typeof(CMCtlPins_Skeleton))]
    public interface ICMCtlPins : IDisposable
    {
        Task<IReadOnlyList<CapnpGen.CMCtlPins.Pin>> GetPinList(CancellationToken cancellationToken_ = default);
        Task<byte> SetPin(string name, CapnpGen.CMCtlPins.PinValue value, CancellationToken cancellationToken_ = default);
        Task<(CapnpGen.CMCtlPins.PinValue, byte)> GetPin(string name, CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<CapnpGen.CMCtlPins.PinResult>> PinActions(IReadOnlyList<CapnpGen.CMCtlPins.PinAction> actions, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d72a49ccb3ac4d0UL)]
    public class CMCtlPins_Proxy : Proxy, ICMCtlPins
    {
        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.Pin>> GetPinList(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPinList.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPinList()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPinList>(d_);
                return (r_.Pins);
            }
        }

        public async Task<byte> SetPin(string name, CapnpGen.CMCtlPins.PinValue value, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_SetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_SetPin()
            {Name = name, Value = value};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_SetPin>(d_);
                return (r_.Error);
            }
        }

        public async Task<(CapnpGen.CMCtlPins.PinValue, byte)> GetPin(string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPin()
            {Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPin>(d_);
                return (r_.Value, r_.Error);
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.PinResult>> PinActions(IReadOnlyList<CapnpGen.CMCtlPins.PinAction> actions, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_PinActions.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_PinActions()
            {Actions = actions};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_PinActions>(d_);
                return (r_.Results);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d72a49ccb3ac4d0UL)]
    public class CMCtlPins_Skeleton : Skeleton<ICMCtlPins>
    {
        public CMCtlPins_Skeleton()
        {
            SetMethodTable(GetPinList, SetPin, GetPin, PinActions);
        }

        public override ulong InterfaceId => 11345311404631180496UL;
        Task<AnswerOrCounterquestion> GetPinList(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetPinList(cancellationToken_), pins =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Result_GetPinList.WRITER>();
                    var r_ = new CapnpGen.CMCtlPins.Result_GetPinList{Pins = pins};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> SetPin(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Params_SetPin>(d_);
                return Impatient.MaybeTailCall(Impl.SetPin(in_.Name, in_.Value, cancellationToken_), error =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Result_SetPin.WRITER>();
                    var r_ = new CapnpGen.CMCtlPins.Result_SetPin{Error = error};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GetPin(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Params_GetPin>(d_);
                return Impatient.MaybeTailCall(Impl.GetPin(in_.Name, cancellationToken_), (value, error) =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Result_GetPin.WRITER>();
                    var r_ = new CapnpGen.CMCtlPins.Result_GetPin{Value = value, Error = error};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> PinActions(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Params_PinActions>(d_);
                return Impatient.MaybeTailCall(Impl.PinActions(in_.Actions, cancellationToken_), results =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Result_PinActions.WRITER>();
                    var r_ = new CapnpGen.CMCtlPins.Result_PinActions{Results = results};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class CMCtlPins
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xea00dbae5e30d175UL)]
        public enum PinType : ushort
        {
            bit,
            @float,
            s32,
            u32,
            unspecified
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdae3af8c1a19b401UL)]
        public enum PinDir : ushort
        {
            @in,
            @out,
            inout,
            unspecified
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaad094cda8b0c270UL)]
        public enum PinAccess : ushort
        {
            getPin,
            setPin
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdd9fcd784aaaa103UL)]
        public class PinValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdd9fcd784aaaa103UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue.value>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Value?.serialize(writer.Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.CMCtlPins.PinValue.value Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public value.READER Value => new value.READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public value.WRITER Value
                {
                    get => Rewrap<value.WRITER>();
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbfc37748bf718f8cUL)]
            public class value : ICapnpSerializable
            {
                public const UInt64 typeId = 0xbfc37748bf718f8cUL;
                public enum WHICH : ushort
                {
                    U = 0,
                    S = 1,
                    F = 2,
                    B = 3,
                    undefined = 65535
                }

                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    switch (reader.which)
                    {
                        case WHICH.U:
                            U = reader.U;
                            break;
                        case WHICH.S:
                            S = reader.S;
                            break;
                        case WHICH.F:
                            F = reader.F;
                            break;
                        case WHICH.B:
                            B = reader.B;
                            break;
                    }

                    applyDefaults();
                }

                private WHICH _which = WHICH.undefined;
                private object _content;
                public WHICH which
                {
                    get => _which;
                    set
                    {
                        if (value == _which)
                            return;
                        _which = value;
                        switch (value)
                        {
                            case WHICH.U:
                                _content = 0;
                                break;
                            case WHICH.S:
                                _content = 0;
                                break;
                            case WHICH.F:
                                _content = 0;
                                break;
                            case WHICH.B:
                                _content = false;
                                break;
                        }
                    }
                }

                public void serialize(WRITER writer)
                {
                    writer.which = which;
                    switch (which)
                    {
                        case WHICH.U:
                            writer.U = U.Value;
                            break;
                        case WHICH.S:
                            writer.S = S.Value;
                            break;
                        case WHICH.F:
                            writer.F = F.Value;
                            break;
                        case WHICH.B:
                            writer.B = B.Value;
                            break;
                    }
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public uint? U
                {
                    get => _which == WHICH.U ? (uint? )_content : null;
                    set
                    {
                        _which = WHICH.U;
                        _content = value;
                    }
                }

                public int? S
                {
                    get => _which == WHICH.S ? (int? )_content : null;
                    set
                    {
                        _which = WHICH.S;
                        _content = value;
                    }
                }

                public double? F
                {
                    get => _which == WHICH.F ? (double? )_content : null;
                    set
                    {
                        _which = WHICH.F;
                        _content = value;
                    }
                }

                public bool? B
                {
                    get => _which == WHICH.B ? (bool? )_content : null;
                    set
                    {
                        _which = WHICH.B;
                        _content = value;
                    }
                }

                public struct READER
                {
                    readonly DeserializerState ctx;
                    public READER(DeserializerState ctx)
                    {
                        this.ctx = ctx;
                    }

                    public static READER create(DeserializerState ctx) => new READER(ctx);
                    public static implicit operator DeserializerState(READER reader) => reader.ctx;
                    public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                    public WHICH which => (WHICH)ctx.ReadDataUShort(32U, (ushort)0);
                    public uint U => which == WHICH.U ? ctx.ReadDataUInt(0UL, 0U) : default;
                    public int S => which == WHICH.S ? ctx.ReadDataInt(0UL, 0) : default;
                    public double F => which == WHICH.F ? ctx.ReadDataDouble(64UL, 0) : default;
                    public bool B => which == WHICH.B ? ctx.ReadDataBool(0UL, false) : default;
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                    }

                    public WHICH which
                    {
                        get => (WHICH)this.ReadDataUShort(32U, (ushort)0);
                        set => this.WriteData(32U, (ushort)value, (ushort)0);
                    }

                    public uint U
                    {
                        get => which == WHICH.U ? this.ReadDataUInt(0UL, 0U) : default;
                        set => this.WriteData(0UL, value, 0U);
                    }

                    public int S
                    {
                        get => which == WHICH.S ? this.ReadDataInt(0UL, 0) : default;
                        set => this.WriteData(0UL, value, 0);
                    }

                    public double F
                    {
                        get => which == WHICH.F ? this.ReadDataDouble(64UL, 0) : default;
                        set => this.WriteData(64UL, value, 0);
                    }

                    public bool B
                    {
                        get => which == WHICH.B ? this.ReadDataBool(0UL, false) : default;
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda66dd5ae92687c1UL)]
        public class Pin : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda66dd5ae92687c1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Type = reader.Type;
                Direction = reader.Direction;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
                writer.Type = Type;
                writer.Direction = Direction;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinType Type
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinDir Direction
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
                public CapnpGen.CMCtlPins.PinType Type => (CapnpGen.CMCtlPins.PinType)ctx.ReadDataUShort(0UL, (ushort)0);
                public CapnpGen.CMCtlPins.PinDir Direction => (CapnpGen.CMCtlPins.PinDir)ctx.ReadDataUShort(16UL, (ushort)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public CapnpGen.CMCtlPins.PinType Type
                {
                    get => (CapnpGen.CMCtlPins.PinType)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public CapnpGen.CMCtlPins.PinDir Direction
                {
                    get => (CapnpGen.CMCtlPins.PinDir)this.ReadDataUShort(16UL, (ushort)0);
                    set => this.WriteData(16UL, (ushort)value, (ushort)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4d244109a99e12dUL)]
        public class PinAction : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe4d244109a99e12dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Action = reader.Action;
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
                writer.Action = Action;
                Value?.serialize(writer.Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinAccess Action
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinValue Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
                public CapnpGen.CMCtlPins.PinAccess Action => (CapnpGen.CMCtlPins.PinAccess)ctx.ReadDataUShort(0UL, (ushort)0);
                public CapnpGen.CMCtlPins.PinValue.READER Value => ctx.ReadStruct(1, CapnpGen.CMCtlPins.PinValue.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public CapnpGen.CMCtlPins.PinAccess Action
                {
                    get => (CapnpGen.CMCtlPins.PinAccess)this.ReadDataUShort(0UL, (ushort)0);
                    set => this.WriteData(0UL, (ushort)value, (ushort)0);
                }

                public CapnpGen.CMCtlPins.PinValue.WRITER Value
                {
                    get => BuildPointer<CapnpGen.CMCtlPins.PinValue.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf7fd1eb39b5245b9UL)]
        public class PinResult : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf7fd1eb39b5245b9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue>(reader.Value);
                Error = reader.Error;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Value?.serialize(writer.Value);
                writer.Error = Error;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.CMCtlPins.PinValue Value
            {
                get;
                set;
            }

            public byte Error
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.CMCtlPins.PinValue.READER Value => ctx.ReadStruct(0, CapnpGen.CMCtlPins.PinValue.READER.create);
                public byte Error => ctx.ReadDataByte(0UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public CapnpGen.CMCtlPins.PinValue.WRITER Value
                {
                    get => BuildPointer<CapnpGen.CMCtlPins.PinValue.WRITER>(0);
                    set => Link(0, value);
                }

                public byte Error
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdea9e268dc142942UL)]
        public class Params_GetPinList : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdea9e268dc142942UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb8bd3b542850b44UL)]
        public class Result_GetPinList : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeb8bd3b542850b44UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Pins = reader.Pins?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.CMCtlPins.Pin>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Pins.Init(Pins, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.CMCtlPins.Pin> Pins
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.CMCtlPins.Pin.READER> Pins => ctx.ReadList(0).Cast(CapnpGen.CMCtlPins.Pin.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.CMCtlPins.Pin.WRITER> Pins
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.CMCtlPins.Pin.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeb146d213d74d281UL)]
        public class Params_SetPin : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeb146d213d74d281UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
                Value?.serialize(writer.Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinValue Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
                public CapnpGen.CMCtlPins.PinValue.READER Value => ctx.ReadStruct(1, CapnpGen.CMCtlPins.PinValue.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 2);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public CapnpGen.CMCtlPins.PinValue.WRITER Value
                {
                    get => BuildPointer<CapnpGen.CMCtlPins.PinValue.WRITER>(1);
                    set => Link(1, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd9898e00cfe516d0UL)]
        public class Result_SetPin : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd9898e00cfe516d0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Error = reader.Error;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Error = Error;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte Error
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public byte Error => ctx.ReadDataByte(0UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public byte Error
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb0d963cae41a6f76UL)]
        public class Params_GetPin : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb0d963cae41a6f76UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc728b2029a95d42fUL)]
        public class Result_GetPin : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc728b2029a95d42fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue>(reader.Value);
                Error = reader.Error;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Value?.serialize(writer.Value);
                writer.Error = Error;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.CMCtlPins.PinValue Value
            {
                get;
                set;
            }

            public byte Error
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.CMCtlPins.PinValue.READER Value => ctx.ReadStruct(0, CapnpGen.CMCtlPins.PinValue.READER.create);
                public byte Error => ctx.ReadDataByte(0UL, (byte)0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public CapnpGen.CMCtlPins.PinValue.WRITER Value
                {
                    get => BuildPointer<CapnpGen.CMCtlPins.PinValue.WRITER>(0);
                    set => Link(0, value);
                }

                public byte Error
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9dc24244b44a41ceUL)]
        public class Params_PinActions : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9dc24244b44a41ceUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Actions = reader.Actions?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.CMCtlPins.PinAction>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Actions.Init(Actions, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.CMCtlPins.PinAction> Actions
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.CMCtlPins.PinAction.READER> Actions => ctx.ReadList(0).Cast(CapnpGen.CMCtlPins.PinAction.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.CMCtlPins.PinAction.WRITER> Actions
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.CMCtlPins.PinAction.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbaee3faa40da7f41UL)]
        public class Result_PinActions : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbaee3faa40da7f41UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Results = reader.Results?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.CMCtlPins.PinResult>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Results.Init(Results, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.CMCtlPins.PinResult> Results
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.CMCtlPins.PinResult.READER> Results => ctx.ReadList(0).Cast(CapnpGen.CMCtlPins.PinResult.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.CMCtlPins.PinResult.WRITER> Results
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.CMCtlPins.PinResult.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9c259414f34fc60UL), Proxy(typeof(CMCtlThreads_Proxy)), Skeleton(typeof(CMCtlThreads_Skeleton))]
    public interface ICMCtlThreads : IDisposable
    {
        Task<IReadOnlyList<CapnpGen.CMCtlThreads.Thread>> GetThreadList(CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9c259414f34fc60UL)]
    public class CMCtlThreads_Proxy : Proxy, ICMCtlThreads
    {
        public async Task<IReadOnlyList<CapnpGen.CMCtlThreads.Thread>> GetThreadList(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlThreads.Params_GetThreadList.WRITER>();
            var arg_ = new CapnpGen.CMCtlThreads.Params_GetThreadList()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12232437674928307296UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlThreads.Result_GetThreadList>(d_);
                return (r_.Threads);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa9c259414f34fc60UL)]
    public class CMCtlThreads_Skeleton : Skeleton<ICMCtlThreads>
    {
        public CMCtlThreads_Skeleton()
        {
            SetMethodTable(GetThreadList);
        }

        public override ulong InterfaceId => 12232437674928307296UL;
        Task<AnswerOrCounterquestion> GetThreadList(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetThreadList(cancellationToken_), threads =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlThreads.Result_GetThreadList.WRITER>();
                    var r_ = new CapnpGen.CMCtlThreads.Result_GetThreadList{Threads = threads};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class CMCtlThreads
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd7a1b637419a6de5UL)]
        public class Thread : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd7a1b637419a6de5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                Period = reader.Period;
                Maxtime = reader.Maxtime;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
                writer.Period = Period;
                writer.Maxtime = Maxtime;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public int Period
            {
                get;
                set;
            }

            public int Maxtime
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
                public int Period => ctx.ReadDataInt(0UL, 0);
                public int Maxtime => ctx.ReadDataInt(32UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public int Period
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public int Maxtime
                {
                    get => this.ReadDataInt(32UL, 0);
                    set => this.WriteData(32UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa82e79edae2bfd4bUL)]
        public class Params_GetThreadList : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa82e79edae2bfd4bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5cad04c7a171dedUL)]
        public class Result_GetThreadList : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe5cad04c7a171dedUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Threads = reader.Threads?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.CMCtlThreads.Thread>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Threads.Init(Threads, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.CMCtlThreads.Thread> Threads
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.CMCtlThreads.Thread.READER> Threads => ctx.ReadList(0).Cast(CapnpGen.CMCtlThreads.Thread.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.CMCtlThreads.Thread.WRITER> Threads
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.CMCtlThreads.Thread.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa527e53265ce17c9UL), Proxy(typeof(CMCtlSampler_Proxy)), Skeleton(typeof(CMCtlSampler_Skeleton))]
    public interface ICMCtlSampler : CapnpGen.ICMCtlPins, CapnpGen.ICMCtlThreads
    {
        Task<bool> StartSampling(IReadOnlyList<string> pins, uint depth, string threadName, string name, CancellationToken cancellationToken_ = default);
        Task<bool> StopSampling(string name, CancellationToken cancellationToken_ = default);
        Task<(CapnpGen.CMCtlSampler.SampleData, bool)> GetSamplesData(string name, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa527e53265ce17c9UL)]
    public class CMCtlSampler_Proxy : Proxy, ICMCtlSampler
    {
        public async Task<bool> StartSampling(IReadOnlyList<string> pins, uint depth, string threadName, string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Params_StartSampling.WRITER>();
            var arg_ = new CapnpGen.CMCtlSampler.Params_StartSampling()
            {Pins = pins, Depth = depth, ThreadName = threadName, Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11900732544968955849UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Result_StartSampling>(d_);
                return (r_.Success);
            }
        }

        public async Task<bool> StopSampling(string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Params_StopSampling.WRITER>();
            var arg_ = new CapnpGen.CMCtlSampler.Params_StopSampling()
            {Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11900732544968955849UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Result_StopSampling>(d_);
                return (r_.Success);
            }
        }

        public async Task<(CapnpGen.CMCtlSampler.SampleData, bool)> GetSamplesData(string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Params_GetSamplesData.WRITER>();
            var arg_ = new CapnpGen.CMCtlSampler.Params_GetSamplesData()
            {Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11900732544968955849UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Result_GetSamplesData>(d_);
                return (r_.Data, r_.Success);
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlThreads.Thread>> GetThreadList(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlThreads.Params_GetThreadList.WRITER>();
            var arg_ = new CapnpGen.CMCtlThreads.Params_GetThreadList()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(12232437674928307296UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlThreads.Result_GetThreadList>(d_);
                return (r_.Threads);
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.Pin>> GetPinList(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPinList.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPinList()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPinList>(d_);
                return (r_.Pins);
            }
        }

        public async Task<byte> SetPin(string name, CapnpGen.CMCtlPins.PinValue value, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_SetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_SetPin()
            {Name = name, Value = value};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_SetPin>(d_);
                return (r_.Error);
            }
        }

        public async Task<(CapnpGen.CMCtlPins.PinValue, byte)> GetPin(string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPin()
            {Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPin>(d_);
                return (r_.Value, r_.Error);
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.PinResult>> PinActions(IReadOnlyList<CapnpGen.CMCtlPins.PinAction> actions, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_PinActions.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_PinActions()
            {Actions = actions};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_PinActions>(d_);
                return (r_.Results);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa527e53265ce17c9UL)]
    public class CMCtlSampler_Skeleton : Skeleton<ICMCtlSampler>
    {
        public CMCtlSampler_Skeleton()
        {
            SetMethodTable(StartSampling, StopSampling, GetSamplesData);
        }

        public override ulong InterfaceId => 11900732544968955849UL;
        Task<AnswerOrCounterquestion> StartSampling(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Params_StartSampling>(d_);
                return Impatient.MaybeTailCall(Impl.StartSampling(in_.Pins, in_.Depth, in_.ThreadName, in_.Name, cancellationToken_), success =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Result_StartSampling.WRITER>();
                    var r_ = new CapnpGen.CMCtlSampler.Result_StartSampling{Success = success};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> StopSampling(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Params_StopSampling>(d_);
                return Impatient.MaybeTailCall(Impl.StopSampling(in_.Name, cancellationToken_), success =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Result_StopSampling.WRITER>();
                    var r_ = new CapnpGen.CMCtlSampler.Result_StopSampling{Success = success};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GetSamplesData(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.CMCtlSampler.Params_GetSamplesData>(d_);
                return Impatient.MaybeTailCall(Impl.GetSamplesData(in_.Name, cancellationToken_), (data, success) =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.CMCtlSampler.Result_GetSamplesData.WRITER>();
                    var r_ = new CapnpGen.CMCtlSampler.Result_GetSamplesData{Data = data, Success = success};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }
    }

    public static class CMCtlSampler
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8e1008604797b631UL)]
        public class SamplePoint : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8e1008604797b631UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                PinName = reader.PinName;
                Value = CapnpSerializable.Create<CapnpGen.CMCtlPins.PinValue>(reader.Value);
                Id = reader.Id;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.PinName = PinName;
                Value?.serialize(writer.Value);
                writer.Id = Id;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string PinName
            {
                get;
                set;
            }

            public CapnpGen.CMCtlPins.PinValue Value
            {
                get;
                set;
            }

            public double Id
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string PinName => ctx.ReadText(0, null);
                public CapnpGen.CMCtlPins.PinValue.READER Value => ctx.ReadStruct(1, CapnpGen.CMCtlPins.PinValue.READER.create);
                public double Id => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 2);
                }

                public string PinName
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public CapnpGen.CMCtlPins.PinValue.WRITER Value
                {
                    get => BuildPointer<CapnpGen.CMCtlPins.PinValue.WRITER>(1);
                    set => Link(1, value);
                }

                public double Id
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9592d223423752afUL)]
        public class SampleData : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9592d223423752afUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Samples = reader.Samples?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.CMCtlSampler.SamplePoint>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Samples.Init(Samples, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.CMCtlSampler.SamplePoint> Samples
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.CMCtlSampler.SamplePoint.READER> Samples => ctx.ReadList(0).Cast(CapnpGen.CMCtlSampler.SamplePoint.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.CMCtlSampler.SamplePoint.WRITER> Samples
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.CMCtlSampler.SamplePoint.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf317bcaf8022e764UL)]
        public class Params_StartSampling : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf317bcaf8022e764UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Pins = reader.Pins;
                Depth = reader.Depth;
                ThreadName = reader.ThreadName;
                Name = reader.Name;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Pins.Init(Pins);
                writer.Depth = Depth;
                writer.ThreadName = ThreadName;
                writer.Name = Name;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<string> Pins
            {
                get;
                set;
            }

            public uint Depth
            {
                get;
                set;
            }

            public string ThreadName
            {
                get;
                set;
            }

            public string Name
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<string> Pins => ctx.ReadList(0).CastText2();
                public uint Depth => ctx.ReadDataUInt(0UL, 0U);
                public string ThreadName => ctx.ReadText(1, null);
                public string Name => ctx.ReadText(2, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 3);
                }

                public ListOfTextSerializer Pins
                {
                    get => BuildPointer<ListOfTextSerializer>(0);
                    set => Link(0, value);
                }

                public uint Depth
                {
                    get => this.ReadDataUInt(0UL, 0U);
                    set => this.WriteData(0UL, value, 0U);
                }

                public string ThreadName
                {
                    get => this.ReadText(1, null);
                    set => this.WriteText(1, value, null);
                }

                public string Name
                {
                    get => this.ReadText(2, null);
                    set => this.WriteText(2, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x86d1112504b04d5aUL)]
        public class Result_StartSampling : ICapnpSerializable
        {
            public const UInt64 typeId = 0x86d1112504b04d5aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Success = reader.Success;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Success = Success;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Success
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Success => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Success
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc56b3ddede95a9a1UL)]
        public class Params_StopSampling : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc56b3ddede95a9a1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9ffa755d663d761cUL)]
        public class Result_StopSampling : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9ffa755d663d761cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Success = reader.Success;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Success = Success;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Success
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Success => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Success
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe045ed5d46a490fbUL)]
        public class Params_GetSamplesData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe045ed5d46a490fbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Name = reader.Name;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Name = Name;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Name
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Name => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Name
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd15de3fb5010c011UL)]
        public class Result_GetSamplesData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd15de3fb5010c011UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = CapnpSerializable.Create<CapnpGen.CMCtlSampler.SampleData>(reader.Data);
                Success = reader.Success;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Data?.serialize(writer.Data);
                writer.Success = Success;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.CMCtlSampler.SampleData Data
            {
                get;
                set;
            }

            public bool Success
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.CMCtlSampler.SampleData.READER Data => ctx.ReadStruct(0, CapnpGen.CMCtlSampler.SampleData.READER.create);
                public bool Success => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public CapnpGen.CMCtlSampler.SampleData.WRITER Data
                {
                    get => BuildPointer<CapnpGen.CMCtlSampler.SampleData.WRITER>(0);
                    set => Link(0, value);
                }

                public bool Success
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xed011cd96cd6be1eUL), Proxy(typeof(OpenCNServerInterface_Proxy)), Skeleton(typeof(OpenCNServerInterface_Skeleton))]
    public interface IOpenCNServerInterface : CapnpGen.ICMCtlPins
    {
        Task Dummy0(CancellationToken cancellationToken_ = default);
        Task Dummy1(CancellationToken cancellationToken_ = default);
        Task SetFeedoptCommitCfg(bool commit, CancellationToken cancellationToken_ = default);
        Task<CapnpGen.OpenCNServerInterface.CyclicData> GetCyclicData(CancellationToken cancellationToken_ = default);
        Task SetLcctSetMachineModeHoming(bool mode, CancellationToken cancellationToken_ = default);
        Task SetLcctSetMachineModeStream(bool mode, CancellationToken cancellationToken_ = default);
        Task SetLcctSetMachineModeJog(bool mode, CancellationToken cancellationToken_ = default);
        Task SetLcctSetMachineModeInactive(bool mode, CancellationToken cancellationToken_ = default);
        Task SetLcctSetMachineModeGCode(bool mode, CancellationToken cancellationToken_ = default);
        Task Dummy9(CancellationToken cancellationToken_ = default);
        Task SetStartHoming(bool mode, CancellationToken cancellationToken_ = default);
        Task SetStopHoming(bool mode, CancellationToken cancellationToken_ = default);
        Task SetHomePositionX(double position, CancellationToken cancellationToken_ = default);
        Task SetHomePositionY(double position, CancellationToken cancellationToken_ = default);
        Task SetHomePositionZ(double position, CancellationToken cancellationToken_ = default);
        Task SetSpeedSpindle(double speed, CancellationToken cancellationToken_ = default);
        Task SetActiveSpindle(bool mode, CancellationToken cancellationToken_ = default);
        Task SetJogX(bool mode, CancellationToken cancellationToken_ = default);
        Task SetJogY(bool mode, CancellationToken cancellationToken_ = default);
        Task SetJogZ(bool mode, CancellationToken cancellationToken_ = default);
        Task SetRelJog(double value, CancellationToken cancellationToken_ = default);
        Task SetPlusJog(bool plus, CancellationToken cancellationToken_ = default);
        Task SetMinusJog(bool minus, CancellationToken cancellationToken_ = default);
        Task SetAbsJog(double value, CancellationToken cancellationToken_ = default);
        Task SetGoJog(bool mode, CancellationToken cancellationToken_ = default);
        Task SetSpeedJog(double speed, CancellationToken cancellationToken_ = default);
        Task SetStopJog(bool mode, CancellationToken cancellationToken_ = default);
        Task SetOffset(double x, double y, double z, double c, CancellationToken cancellationToken_ = default);
        Task Dummy28(CancellationToken cancellationToken_ = default);
        Task Dummy29(CancellationToken cancellationToken_ = default);
        Task SetStartStream(bool mode, CancellationToken cancellationToken_ = default);
        Task SetStopStream(bool mode, CancellationToken cancellationToken_ = default);
        Task SetGcodeStart(bool mode, CancellationToken cancellationToken_ = default);
        Task SetGcodePause(bool mode, CancellationToken cancellationToken_ = default);
        Task SetFaultReset(bool reset, CancellationToken cancellationToken_ = default);
        Task SetFeedrateScale(double scale, CancellationToken cancellationToken_ = default);
        Task SetFeedoptReset(bool reset, CancellationToken cancellationToken_ = default);
        Task<string> ReadLog(CancellationToken cancellationToken_ = default);
        Task SetFeedoptConfig(CapnpGen.OpenCNServerInterface.FeedOptCfg config, CancellationToken cancellationToken_ = default);
        Task<CapnpGen.OpenCNServerInterface.FeedOptCfg> GetFeedoptConfig(CancellationToken cancellationToken_ = default);
        Task<bool> ToolpathStartChannel(int sampleRate, CancellationToken cancellationToken_ = default);
        Task ToolpathStopChannel(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<CapnpGen.OpenCNServerInterface.Sample>> ToolpathReadSamples(CancellationToken cancellationToken_ = default);
        Task<int> SendFileParam(string fileName, uint size, int fileOp, CancellationToken cancellationToken_ = default);
        Task<int> SendFileData(IReadOnlyList<byte> data, CancellationToken cancellationToken_ = default);
        Task<bool> PathExist(string path, CancellationToken cancellationToken_ = default);
        Task<bool> CreateFolder(string folderPath, CancellationToken cancellationToken_ = default);
        Task SetLoadStream(bool mode, CancellationToken cancellationToken_ = default);
        Task SetPauseStream(bool mode, CancellationToken cancellationToken_ = default);
        Task SamplerNewFile(CancellationToken cancellationToken_ = default);
        Task<IReadOnlyList<byte>> GetFileData(CancellationToken cancellationToken_ = default);
        Task SetSamplerDownloadFile(bool download, CancellationToken cancellationToken_ = default);
        Task SetSpindleThreshold(int percent, CancellationToken cancellationToken_ = default);
        Task StartJog(byte axis, double target, bool relative, CancellationToken cancellationToken_ = default);
        Task SetFreeTool(bool state, CancellationToken cancellationToken_ = default);
        Task SetFreePalette(bool state, CancellationToken cancellationToken_ = default);
        Task<(CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit)> GetMachineLimits(CancellationToken cancellationToken_ = default);
        Task<double> GetSpindleOverride(CancellationToken cancellationToken_ = default);
        Task SetSpindleOverride(double @override, CancellationToken cancellationToken_ = default);
        Task<bool> GetLubStatus(CancellationToken cancellationToken_ = default);
        Task SetLubForce(bool lubForce, CancellationToken cancellationToken_ = default);
        Task SetLubWhenMilling(bool lubWhenMilling, CancellationToken cancellationToken_ = default);
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xed011cd96cd6be1eUL)]
    public class OpenCNServerInterface_Proxy : Proxy, IOpenCNServerInterface
    {
        public async Task Dummy0(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_Dummy0.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_Dummy0()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_Dummy0>(d_);
                return;
            }
        }

        public async Task Dummy1(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_Dummy1.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_Dummy1()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_Dummy1>(d_);
                return;
            }
        }

        public async Task SetFeedoptCommitCfg(bool commit, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFeedoptCommitCfg.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFeedoptCommitCfg()
            {Commit = commit};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFeedoptCommitCfg>(d_);
                return;
            }
        }

        public async Task<CapnpGen.OpenCNServerInterface.CyclicData> GetCyclicData(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetCyclicData.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetCyclicData()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetCyclicData>(d_);
                return (r_.Data);
            }
        }

        public async Task SetLcctSetMachineModeHoming(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeHoming.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeHoming()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 4, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeHoming>(d_);
                return;
            }
        }

        public async Task SetLcctSetMachineModeStream(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeStream.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeStream()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 5, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeStream>(d_);
                return;
            }
        }

        public async Task SetLcctSetMachineModeJog(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeJog()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 6, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeJog>(d_);
                return;
            }
        }

        public async Task SetLcctSetMachineModeInactive(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeInactive.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeInactive()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 7, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeInactive>(d_);
                return;
            }
        }

        public async Task SetLcctSetMachineModeGCode(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeGCode.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeGCode()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 8, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeGCode>(d_);
                return;
            }
        }

        public async Task Dummy9(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_Dummy9.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_Dummy9()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 9, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_Dummy9>(d_);
                return;
            }
        }

        public async Task SetStartHoming(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetStartHoming.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetStartHoming()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 10, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetStartHoming>(d_);
                return;
            }
        }

        public async Task SetStopHoming(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetStopHoming.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetStopHoming()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 11, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetStopHoming>(d_);
                return;
            }
        }

        public async Task SetHomePositionX(double position, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetHomePositionX.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetHomePositionX()
            {Position = position};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 12, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetHomePositionX>(d_);
                return;
            }
        }

        public async Task SetHomePositionY(double position, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetHomePositionY.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetHomePositionY()
            {Position = position};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 13, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetHomePositionY>(d_);
                return;
            }
        }

        public async Task SetHomePositionZ(double position, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetHomePositionZ.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetHomePositionZ()
            {Position = position};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 14, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetHomePositionZ>(d_);
                return;
            }
        }

        public async Task SetSpeedSpindle(double speed, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetSpeedSpindle.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetSpeedSpindle()
            {Speed = speed};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 15, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetSpeedSpindle>(d_);
                return;
            }
        }

        public async Task SetActiveSpindle(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetActiveSpindle.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetActiveSpindle()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 16, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetActiveSpindle>(d_);
                return;
            }
        }

        public async Task SetJogX(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetJogX.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetJogX()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 17, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetJogX>(d_);
                return;
            }
        }

        public async Task SetJogY(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetJogY.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetJogY()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 18, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetJogY>(d_);
                return;
            }
        }

        public async Task SetJogZ(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetJogZ.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetJogZ()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 19, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetJogZ>(d_);
                return;
            }
        }

        public async Task SetRelJog(double value, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetRelJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetRelJog()
            {Value = value};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 20, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetRelJog>(d_);
                return;
            }
        }

        public async Task SetPlusJog(bool plus, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetPlusJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetPlusJog()
            {Plus = plus};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 21, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetPlusJog>(d_);
                return;
            }
        }

        public async Task SetMinusJog(bool minus, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetMinusJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetMinusJog()
            {Minus = minus};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 22, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetMinusJog>(d_);
                return;
            }
        }

        public async Task SetAbsJog(double value, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetAbsJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetAbsJog()
            {Value = value};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 23, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetAbsJog>(d_);
                return;
            }
        }

        public async Task SetGoJog(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetGoJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetGoJog()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 24, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetGoJog>(d_);
                return;
            }
        }

        public async Task SetSpeedJog(double speed, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetSpeedJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetSpeedJog()
            {Speed = speed};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 25, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetSpeedJog>(d_);
                return;
            }
        }

        public async Task SetStopJog(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetStopJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetStopJog()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 26, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetStopJog>(d_);
                return;
            }
        }

        public async Task SetOffset(double x, double y, double z, double c, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetOffset.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetOffset()
            {X = x, Y = y, Z = z, C = c};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 27, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetOffset>(d_);
                return;
            }
        }

        public async Task Dummy28(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_Dummy28.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_Dummy28()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 28, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_Dummy28>(d_);
                return;
            }
        }

        public async Task Dummy29(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_Dummy29.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_Dummy29()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 29, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_Dummy29>(d_);
                return;
            }
        }

        public async Task SetStartStream(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetStartStream.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetStartStream()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 30, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetStartStream>(d_);
                return;
            }
        }

        public async Task SetStopStream(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetStopStream.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetStopStream()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 31, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetStopStream>(d_);
                return;
            }
        }

        public async Task SetGcodeStart(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetGcodeStart.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetGcodeStart()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 32, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetGcodeStart>(d_);
                return;
            }
        }

        public async Task SetGcodePause(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetGcodePause.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetGcodePause()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 33, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetGcodePause>(d_);
                return;
            }
        }

        public async Task SetFaultReset(bool reset, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFaultReset.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFaultReset()
            {Reset = reset};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 34, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFaultReset>(d_);
                return;
            }
        }

        public async Task SetFeedrateScale(double scale, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFeedrateScale.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFeedrateScale()
            {Scale = scale};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 35, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFeedrateScale>(d_);
                return;
            }
        }

        public async Task SetFeedoptReset(bool reset, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFeedoptReset.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFeedoptReset()
            {Reset = reset};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 36, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFeedoptReset>(d_);
                return;
            }
        }

        public async Task<string> ReadLog(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_ReadLog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_ReadLog()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 37, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_ReadLog>(d_);
                return (r_.Message);
            }
        }

        public async Task SetFeedoptConfig(CapnpGen.OpenCNServerInterface.FeedOptCfg config, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFeedoptConfig.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFeedoptConfig()
            {Config = config};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 38, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFeedoptConfig>(d_);
                return;
            }
        }

        public async Task<CapnpGen.OpenCNServerInterface.FeedOptCfg> GetFeedoptConfig(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetFeedoptConfig.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetFeedoptConfig()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 39, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetFeedoptConfig>(d_);
                return (r_.Config);
            }
        }

        public async Task<bool> ToolpathStartChannel(int sampleRate, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_ToolpathStartChannel.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_ToolpathStartChannel()
            {SampleRate = sampleRate};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 40, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_ToolpathStartChannel>(d_);
                return (r_.Result);
            }
        }

        public async Task ToolpathStopChannel(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_ToolpathStopChannel.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_ToolpathStopChannel()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 41, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_ToolpathStopChannel>(d_);
                return;
            }
        }

        public async Task<IReadOnlyList<CapnpGen.OpenCNServerInterface.Sample>> ToolpathReadSamples(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_ToolpathReadSamples.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_ToolpathReadSamples()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 42, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_ToolpathReadSamples>(d_);
                return (r_.Samples);
            }
        }

        public async Task<int> SendFileParam(string fileName, uint size, int fileOp, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SendFileParam.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SendFileParam()
            {FileName = fileName, Size = size, FileOp = fileOp};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 43, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SendFileParam>(d_);
                return (r_.Result);
            }
        }

        public async Task<int> SendFileData(IReadOnlyList<byte> data, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SendFileData.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SendFileData()
            {Data = data};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 44, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SendFileData>(d_);
                return (r_.Result);
            }
        }

        public async Task<bool> PathExist(string path, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_PathExist.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_PathExist()
            {Path = path};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 45, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_PathExist>(d_);
                return (r_.Result);
            }
        }

        public async Task<bool> CreateFolder(string folderPath, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_CreateFolder.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_CreateFolder()
            {FolderPath = folderPath};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 46, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_CreateFolder>(d_);
                return (r_.Result);
            }
        }

        public async Task SetLoadStream(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLoadStream.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLoadStream()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 47, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLoadStream>(d_);
                return;
            }
        }

        public async Task SetPauseStream(bool mode, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetPauseStream.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetPauseStream()
            {Mode = mode};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 48, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetPauseStream>(d_);
                return;
            }
        }

        public async Task SamplerNewFile(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SamplerNewFile.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SamplerNewFile()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 49, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SamplerNewFile>(d_);
                return;
            }
        }

        public async Task<IReadOnlyList<byte>> GetFileData(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetFileData.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetFileData()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 50, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetFileData>(d_);
                return (r_.Data);
            }
        }

        public async Task SetSamplerDownloadFile(bool download, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetSamplerDownloadFile.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetSamplerDownloadFile()
            {Download = download};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 51, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetSamplerDownloadFile>(d_);
                return;
            }
        }

        public async Task SetSpindleThreshold(int percent, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetSpindleThreshold.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetSpindleThreshold()
            {Percent = percent};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 52, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetSpindleThreshold>(d_);
                return;
            }
        }

        public async Task StartJog(byte axis, double target, bool relative, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_StartJog.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_StartJog()
            {Axis = axis, Target = target, Relative = relative};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 53, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_StartJog>(d_);
                return;
            }
        }

        public async Task SetFreeTool(bool state, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFreeTool.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFreeTool()
            {State = state};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 54, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFreeTool>(d_);
                return;
            }
        }

        public async Task SetFreePalette(bool state, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetFreePalette.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetFreePalette()
            {State = state};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 55, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetFreePalette>(d_);
                return;
            }
        }

        public async Task<(CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit, CapnpGen.OpenCNServerInterface.Limit)> GetMachineLimits(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetMachineLimits.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetMachineLimits()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 56, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetMachineLimits>(d_);
                return (r_.X, r_.Y, r_.Z, r_.B, r_.C);
            }
        }

        public async Task<double> GetSpindleOverride(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetSpindleOverride.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetSpindleOverride()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 57, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetSpindleOverride>(d_);
                return (r_.Result);
            }
        }

        public async Task SetSpindleOverride(double @override, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetSpindleOverride.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetSpindleOverride()
            {Override = @override};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 58, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetSpindleOverride>(d_);
                return;
            }
        }

        public async Task<bool> GetLubStatus(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_GetLubStatus.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_GetLubStatus()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 59, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_GetLubStatus>(d_);
                return (r_.Result);
            }
        }

        public async Task SetLubForce(bool lubForce, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLubForce.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLubForce()
            {LubForce = lubForce};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 60, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLubForce>(d_);
                return;
            }
        }

        public async Task SetLubWhenMilling(bool lubWhenMilling, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Params_SetLubWhenMilling.WRITER>();
            var arg_ = new CapnpGen.OpenCNServerInterface.Params_SetLubWhenMilling()
            {LubWhenMilling = lubWhenMilling};
            arg_?.serialize(in_);
            using (var d_ = await Call(17077962982125125150UL, 61, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Result_SetLubWhenMilling>(d_);
                return;
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.Pin>> GetPinList(CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPinList.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPinList()
            {};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 0, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPinList>(d_);
                return (r_.Pins);
            }
        }

        public async Task<byte> SetPin(string name, CapnpGen.CMCtlPins.PinValue value, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_SetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_SetPin()
            {Name = name, Value = value};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 1, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_SetPin>(d_);
                return (r_.Error);
            }
        }

        public async Task<(CapnpGen.CMCtlPins.PinValue, byte)> GetPin(string name, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_GetPin.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_GetPin()
            {Name = name};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 2, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_GetPin>(d_);
                return (r_.Value, r_.Error);
            }
        }

        public async Task<IReadOnlyList<CapnpGen.CMCtlPins.PinResult>> PinActions(IReadOnlyList<CapnpGen.CMCtlPins.PinAction> actions, CancellationToken cancellationToken_ = default)
        {
            var in_ = SerializerState.CreateForRpc<CapnpGen.CMCtlPins.Params_PinActions.WRITER>();
            var arg_ = new CapnpGen.CMCtlPins.Params_PinActions()
            {Actions = actions};
            arg_?.serialize(in_);
            using (var d_ = await Call(11345311404631180496UL, 3, in_.Rewrap<DynamicSerializerState>(), false, cancellationToken_).WhenReturned)
            {
                var r_ = CapnpSerializable.Create<CapnpGen.CMCtlPins.Result_PinActions>(d_);
                return (r_.Results);
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xed011cd96cd6be1eUL)]
    public class OpenCNServerInterface_Skeleton : Skeleton<IOpenCNServerInterface>
    {
        public OpenCNServerInterface_Skeleton()
        {
            SetMethodTable(Dummy0, Dummy1, SetFeedoptCommitCfg, GetCyclicData, SetLcctSetMachineModeHoming, SetLcctSetMachineModeStream, SetLcctSetMachineModeJog, SetLcctSetMachineModeInactive, SetLcctSetMachineModeGCode, Dummy9, SetStartHoming, SetStopHoming, SetHomePositionX, SetHomePositionY, SetHomePositionZ, SetSpeedSpindle, SetActiveSpindle, SetJogX, SetJogY, SetJogZ, SetRelJog, SetPlusJog, SetMinusJog, SetAbsJog, SetGoJog, SetSpeedJog, SetStopJog, SetOffset, Dummy28, Dummy29, SetStartStream, SetStopStream, SetGcodeStart, SetGcodePause, SetFaultReset, SetFeedrateScale, SetFeedoptReset, ReadLog, SetFeedoptConfig, GetFeedoptConfig, ToolpathStartChannel, ToolpathStopChannel, ToolpathReadSamples, SendFileParam, SendFileData, PathExist, CreateFolder, SetLoadStream, SetPauseStream, SamplerNewFile, GetFileData, SetSamplerDownloadFile, SetSpindleThreshold, StartJog, SetFreeTool, SetFreePalette, GetMachineLimits, GetSpindleOverride, SetSpindleOverride, GetLubStatus, SetLubForce, SetLubWhenMilling);
        }

        public override ulong InterfaceId => 17077962982125125150UL;
        async Task<AnswerOrCounterquestion> Dummy0(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Dummy0(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_Dummy0.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Dummy1(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Dummy1(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_Dummy1.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFeedoptCommitCfg(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFeedoptCommitCfg>(d_);
                await Impl.SetFeedoptCommitCfg(in_.Commit, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFeedoptCommitCfg.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetCyclicData(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetCyclicData(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetCyclicData.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetCyclicData{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetLcctSetMachineModeHoming(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeHoming>(d_);
                await Impl.SetLcctSetMachineModeHoming(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeHoming.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetLcctSetMachineModeStream(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeStream>(d_);
                await Impl.SetLcctSetMachineModeStream(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeStream.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetLcctSetMachineModeJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeJog>(d_);
                await Impl.SetLcctSetMachineModeJog(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetLcctSetMachineModeInactive(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeInactive>(d_);
                await Impl.SetLcctSetMachineModeInactive(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeInactive.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetLcctSetMachineModeGCode(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLcctSetMachineModeGCode>(d_);
                await Impl.SetLcctSetMachineModeGCode(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLcctSetMachineModeGCode.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Dummy9(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Dummy9(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_Dummy9.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetStartHoming(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetStartHoming>(d_);
                await Impl.SetStartHoming(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetStartHoming.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetStopHoming(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetStopHoming>(d_);
                await Impl.SetStopHoming(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetStopHoming.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetHomePositionX(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetHomePositionX>(d_);
                await Impl.SetHomePositionX(in_.Position, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetHomePositionX.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetHomePositionY(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetHomePositionY>(d_);
                await Impl.SetHomePositionY(in_.Position, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetHomePositionY.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetHomePositionZ(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetHomePositionZ>(d_);
                await Impl.SetHomePositionZ(in_.Position, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetHomePositionZ.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetSpeedSpindle(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetSpeedSpindle>(d_);
                await Impl.SetSpeedSpindle(in_.Speed, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetSpeedSpindle.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetActiveSpindle(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetActiveSpindle>(d_);
                await Impl.SetActiveSpindle(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetActiveSpindle.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetJogX(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetJogX>(d_);
                await Impl.SetJogX(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetJogX.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetJogY(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetJogY>(d_);
                await Impl.SetJogY(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetJogY.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetJogZ(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetJogZ>(d_);
                await Impl.SetJogZ(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetJogZ.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetRelJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetRelJog>(d_);
                await Impl.SetRelJog(in_.Value, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetRelJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetPlusJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetPlusJog>(d_);
                await Impl.SetPlusJog(in_.Plus, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetPlusJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetMinusJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetMinusJog>(d_);
                await Impl.SetMinusJog(in_.Minus, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetMinusJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetAbsJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetAbsJog>(d_);
                await Impl.SetAbsJog(in_.Value, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetAbsJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetGoJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetGoJog>(d_);
                await Impl.SetGoJog(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetGoJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetSpeedJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetSpeedJog>(d_);
                await Impl.SetSpeedJog(in_.Speed, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetSpeedJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetStopJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetStopJog>(d_);
                await Impl.SetStopJog(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetStopJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetOffset(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetOffset>(d_);
                await Impl.SetOffset(in_.X, in_.Y, in_.Z, in_.C, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetOffset.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Dummy28(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Dummy28(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_Dummy28.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> Dummy29(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.Dummy29(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_Dummy29.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetStartStream(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetStartStream>(d_);
                await Impl.SetStartStream(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetStartStream.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetStopStream(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetStopStream>(d_);
                await Impl.SetStopStream(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetStopStream.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetGcodeStart(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetGcodeStart>(d_);
                await Impl.SetGcodeStart(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetGcodeStart.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetGcodePause(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetGcodePause>(d_);
                await Impl.SetGcodePause(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetGcodePause.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFaultReset(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFaultReset>(d_);
                await Impl.SetFaultReset(in_.Reset, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFaultReset.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFeedrateScale(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFeedrateScale>(d_);
                await Impl.SetFeedrateScale(in_.Scale, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFeedrateScale.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFeedoptReset(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFeedoptReset>(d_);
                await Impl.SetFeedoptReset(in_.Reset, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFeedoptReset.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> ReadLog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ReadLog(cancellationToken_), message =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_ReadLog.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_ReadLog{Message = message};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetFeedoptConfig(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFeedoptConfig>(d_);
                await Impl.SetFeedoptConfig(in_.Config, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFeedoptConfig.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetFeedoptConfig(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetFeedoptConfig(cancellationToken_), config =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetFeedoptConfig.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetFeedoptConfig{Config = config};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> ToolpathStartChannel(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_ToolpathStartChannel>(d_);
                return Impatient.MaybeTailCall(Impl.ToolpathStartChannel(in_.SampleRate, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_ToolpathStartChannel.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_ToolpathStartChannel{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> ToolpathStopChannel(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.ToolpathStopChannel(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_ToolpathStopChannel.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> ToolpathReadSamples(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.ToolpathReadSamples(cancellationToken_), samples =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_ToolpathReadSamples.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_ToolpathReadSamples{Samples = samples};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> SendFileParam(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SendFileParam>(d_);
                return Impatient.MaybeTailCall(Impl.SendFileParam(in_.FileName, in_.Size, in_.FileOp, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SendFileParam.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_SendFileParam{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> SendFileData(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SendFileData>(d_);
                return Impatient.MaybeTailCall(Impl.SendFileData(in_.Data, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SendFileData.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_SendFileData{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> PathExist(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_PathExist>(d_);
                return Impatient.MaybeTailCall(Impl.PathExist(in_.Path, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_PathExist.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_PathExist{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> CreateFolder(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_CreateFolder>(d_);
                return Impatient.MaybeTailCall(Impl.CreateFolder(in_.FolderPath, cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_CreateFolder.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_CreateFolder{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetLoadStream(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLoadStream>(d_);
                await Impl.SetLoadStream(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLoadStream.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetPauseStream(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetPauseStream>(d_);
                await Impl.SetPauseStream(in_.Mode, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetPauseStream.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SamplerNewFile(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                await Impl.SamplerNewFile(cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SamplerNewFile.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetFileData(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetFileData(cancellationToken_), data =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetFileData.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetFileData{Data = data};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetSamplerDownloadFile(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetSamplerDownloadFile>(d_);
                await Impl.SetSamplerDownloadFile(in_.Download, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetSamplerDownloadFile.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetSpindleThreshold(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetSpindleThreshold>(d_);
                await Impl.SetSpindleThreshold(in_.Percent, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetSpindleThreshold.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> StartJog(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_StartJog>(d_);
                await Impl.StartJog(in_.Axis, in_.Target, in_.Relative, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_StartJog.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFreeTool(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFreeTool>(d_);
                await Impl.SetFreeTool(in_.State, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFreeTool.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetFreePalette(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetFreePalette>(d_);
                await Impl.SetFreePalette(in_.State, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetFreePalette.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetMachineLimits(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetMachineLimits(cancellationToken_), (x, y, z, b, c) =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetMachineLimits.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetMachineLimits{X = x, Y = y, Z = z, B = b, C = c};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        Task<AnswerOrCounterquestion> GetSpindleOverride(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetSpindleOverride(cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetSpindleOverride.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetSpindleOverride{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetSpindleOverride(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetSpindleOverride>(d_);
                await Impl.SetSpindleOverride(in_.Override, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetSpindleOverride.WRITER>();
                return s_;
            }
        }

        Task<AnswerOrCounterquestion> GetLubStatus(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                return Impatient.MaybeTailCall(Impl.GetLubStatus(cancellationToken_), result =>
                {
                    var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_GetLubStatus.WRITER>();
                    var r_ = new CapnpGen.OpenCNServerInterface.Result_GetLubStatus{Result = result};
                    r_.serialize(s_);
                    return s_;
                }

                );
            }
        }

        async Task<AnswerOrCounterquestion> SetLubForce(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLubForce>(d_);
                await Impl.SetLubForce(in_.LubForce, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLubForce.WRITER>();
                return s_;
            }
        }

        async Task<AnswerOrCounterquestion> SetLubWhenMilling(DeserializerState d_, CancellationToken cancellationToken_)
        {
            using (d_)
            {
                var in_ = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Params_SetLubWhenMilling>(d_);
                await Impl.SetLubWhenMilling(in_.LubWhenMilling, cancellationToken_);
                var s_ = SerializerState.CreateForRpc<CapnpGen.OpenCNServerInterface.Result_SetLubWhenMilling.WRITER>();
                return s_;
            }
        }
    }

    public static class OpenCNServerInterface
    {
        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xeadc061fd25ff08bUL)]
        public class CyclicData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xeadc061fd25ff08bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                FeedoptStepDt = reader.FeedoptStepDt;
                FeedoptQueueSize = reader.FeedoptQueueSize;
                FeedoptProgress = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Progress>(reader.FeedoptProgress);
                HomingFinished = reader.HomingFinished;
                StreamFinished = reader.StreamFinished;
                StreamRunning = reader.StreamRunning;
                JogFinished = reader.JogFinished;
                GcodeFinished = reader.GcodeFinished;
                GcodeRunning = reader.GcodeRunning;
                CurrentPosition = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Position>(reader.CurrentPosition);
                SpindleVelocity = reader.SpindleVelocity;
                AxisMode = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMode>(reader.AxisMode);
                Homed = reader.Homed;
                FeedoptUsActive = reader.FeedoptUsActive;
                FeedoptRtActive = reader.FeedoptRtActive;
                FeedoptReady = reader.FeedoptReady;
                StreamerFIFO = reader.StreamerFIFO;
                MachineMode = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.MachineMode>(reader.MachineMode);
                FeedoptQueueMax = reader.FeedoptQueueMax;
                CurrentGCodeLine = reader.CurrentGCodeLine;
                MachineState = reader.MachineState;
                XLoad = reader.XLoad;
                YLoad = reader.YLoad;
                ZLoad = reader.ZLoad;
                BLoad = reader.BLoad;
                CLoad = reader.CLoad;
                SLoad = reader.SLoad;
                SpindleTemp = reader.SpindleTemp;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.FeedoptStepDt = FeedoptStepDt;
                writer.FeedoptQueueSize = FeedoptQueueSize;
                FeedoptProgress?.serialize(writer.FeedoptProgress);
                writer.HomingFinished = HomingFinished;
                writer.StreamFinished = StreamFinished;
                writer.StreamRunning = StreamRunning;
                writer.JogFinished = JogFinished;
                writer.GcodeFinished = GcodeFinished;
                writer.GcodeRunning = GcodeRunning;
                CurrentPosition?.serialize(writer.CurrentPosition);
                writer.SpindleVelocity = SpindleVelocity;
                AxisMode?.serialize(writer.AxisMode);
                writer.Homed = Homed;
                writer.FeedoptUsActive = FeedoptUsActive;
                writer.FeedoptRtActive = FeedoptRtActive;
                writer.FeedoptReady = FeedoptReady;
                writer.StreamerFIFO = StreamerFIFO;
                MachineMode?.serialize(writer.MachineMode);
                writer.FeedoptQueueMax = FeedoptQueueMax;
                writer.CurrentGCodeLine = CurrentGCodeLine;
                writer.MachineState = MachineState;
                writer.XLoad = XLoad;
                writer.YLoad = YLoad;
                writer.ZLoad = ZLoad;
                writer.BLoad = BLoad;
                writer.CLoad = CLoad;
                writer.SLoad = SLoad;
                writer.SpindleTemp = SpindleTemp;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double FeedoptStepDt
            {
                get;
                set;
            }

            public int FeedoptQueueSize
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Progress FeedoptProgress
            {
                get;
                set;
            }

            public bool HomingFinished
            {
                get;
                set;
            }

            public bool StreamFinished
            {
                get;
                set;
            }

            public bool StreamRunning
            {
                get;
                set;
            }

            public bool JogFinished
            {
                get;
                set;
            }

            public bool GcodeFinished
            {
                get;
                set;
            }

            public bool GcodeRunning
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Position CurrentPosition
            {
                get;
                set;
            }

            public double SpindleVelocity
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.AxisMode AxisMode
            {
                get;
                set;
            }

            public bool Homed
            {
                get;
                set;
            }

            public bool FeedoptUsActive
            {
                get;
                set;
            }

            public bool FeedoptRtActive
            {
                get;
                set;
            }

            public bool FeedoptReady
            {
                get;
                set;
            }

            public int StreamerFIFO
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.MachineMode MachineMode
            {
                get;
                set;
            }

            public int FeedoptQueueMax
            {
                get;
                set;
            }

            public int CurrentGCodeLine
            {
                get;
                set;
            }

            public uint MachineState
            {
                get;
                set;
            }

            public double XLoad
            {
                get;
                set;
            }

            public double YLoad
            {
                get;
                set;
            }

            public double ZLoad
            {
                get;
                set;
            }

            public double BLoad
            {
                get;
                set;
            }

            public double CLoad
            {
                get;
                set;
            }

            public double SLoad
            {
                get;
                set;
            }

            public double SpindleTemp
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double FeedoptStepDt => ctx.ReadDataDouble(0UL, 0);
                public int FeedoptQueueSize => ctx.ReadDataInt(64UL, 0);
                public CapnpGen.OpenCNServerInterface.Progress.READER FeedoptProgress => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.Progress.READER.create);
                public bool HomingFinished => ctx.ReadDataBool(96UL, false);
                public bool StreamFinished => ctx.ReadDataBool(97UL, false);
                public bool StreamRunning => ctx.ReadDataBool(98UL, false);
                public bool JogFinished => ctx.ReadDataBool(99UL, false);
                public bool GcodeFinished => ctx.ReadDataBool(100UL, false);
                public bool GcodeRunning => ctx.ReadDataBool(101UL, false);
                public CapnpGen.OpenCNServerInterface.Position.READER CurrentPosition => ctx.ReadStruct(1, CapnpGen.OpenCNServerInterface.Position.READER.create);
                public double SpindleVelocity => ctx.ReadDataDouble(128UL, 0);
                public CapnpGen.OpenCNServerInterface.AxisMode.READER AxisMode => ctx.ReadStruct(2, CapnpGen.OpenCNServerInterface.AxisMode.READER.create);
                public bool Homed => ctx.ReadDataBool(102UL, false);
                public bool FeedoptUsActive => ctx.ReadDataBool(103UL, false);
                public bool FeedoptRtActive => ctx.ReadDataBool(104UL, false);
                public bool FeedoptReady => ctx.ReadDataBool(105UL, false);
                public int StreamerFIFO => ctx.ReadDataInt(192UL, 0);
                public CapnpGen.OpenCNServerInterface.MachineMode.READER MachineMode => ctx.ReadStruct(3, CapnpGen.OpenCNServerInterface.MachineMode.READER.create);
                public int FeedoptQueueMax => ctx.ReadDataInt(224UL, 0);
                public int CurrentGCodeLine => ctx.ReadDataInt(256UL, 0);
                public uint MachineState => ctx.ReadDataUInt(288UL, 0U);
                public double XLoad => ctx.ReadDataDouble(320UL, 0);
                public double YLoad => ctx.ReadDataDouble(384UL, 0);
                public double ZLoad => ctx.ReadDataDouble(448UL, 0);
                public double BLoad => ctx.ReadDataDouble(512UL, 0);
                public double CLoad => ctx.ReadDataDouble(576UL, 0);
                public double SLoad => ctx.ReadDataDouble(640UL, 0);
                public double SpindleTemp => ctx.ReadDataDouble(704UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(12, 4);
                }

                public double FeedoptStepDt
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public int FeedoptQueueSize
                {
                    get => this.ReadDataInt(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public CapnpGen.OpenCNServerInterface.Progress.WRITER FeedoptProgress
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Progress.WRITER>(0);
                    set => Link(0, value);
                }

                public bool HomingFinished
                {
                    get => this.ReadDataBool(96UL, false);
                    set => this.WriteData(96UL, value, false);
                }

                public bool StreamFinished
                {
                    get => this.ReadDataBool(97UL, false);
                    set => this.WriteData(97UL, value, false);
                }

                public bool StreamRunning
                {
                    get => this.ReadDataBool(98UL, false);
                    set => this.WriteData(98UL, value, false);
                }

                public bool JogFinished
                {
                    get => this.ReadDataBool(99UL, false);
                    set => this.WriteData(99UL, value, false);
                }

                public bool GcodeFinished
                {
                    get => this.ReadDataBool(100UL, false);
                    set => this.WriteData(100UL, value, false);
                }

                public bool GcodeRunning
                {
                    get => this.ReadDataBool(101UL, false);
                    set => this.WriteData(101UL, value, false);
                }

                public CapnpGen.OpenCNServerInterface.Position.WRITER CurrentPosition
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Position.WRITER>(1);
                    set => Link(1, value);
                }

                public double SpindleVelocity
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public CapnpGen.OpenCNServerInterface.AxisMode.WRITER AxisMode
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMode.WRITER>(2);
                    set => Link(2, value);
                }

                public bool Homed
                {
                    get => this.ReadDataBool(102UL, false);
                    set => this.WriteData(102UL, value, false);
                }

                public bool FeedoptUsActive
                {
                    get => this.ReadDataBool(103UL, false);
                    set => this.WriteData(103UL, value, false);
                }

                public bool FeedoptRtActive
                {
                    get => this.ReadDataBool(104UL, false);
                    set => this.WriteData(104UL, value, false);
                }

                public bool FeedoptReady
                {
                    get => this.ReadDataBool(105UL, false);
                    set => this.WriteData(105UL, value, false);
                }

                public int StreamerFIFO
                {
                    get => this.ReadDataInt(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public CapnpGen.OpenCNServerInterface.MachineMode.WRITER MachineMode
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.MachineMode.WRITER>(3);
                    set => Link(3, value);
                }

                public int FeedoptQueueMax
                {
                    get => this.ReadDataInt(224UL, 0);
                    set => this.WriteData(224UL, value, 0);
                }

                public int CurrentGCodeLine
                {
                    get => this.ReadDataInt(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public uint MachineState
                {
                    get => this.ReadDataUInt(288UL, 0U);
                    set => this.WriteData(288UL, value, 0U);
                }

                public double XLoad
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }

                public double YLoad
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double ZLoad
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public double BLoad
                {
                    get => this.ReadDataDouble(512UL, 0);
                    set => this.WriteData(512UL, value, 0);
                }

                public double CLoad
                {
                    get => this.ReadDataDouble(576UL, 0);
                    set => this.WriteData(576UL, value, 0);
                }

                public double SLoad
                {
                    get => this.ReadDataDouble(640UL, 0);
                    set => this.WriteData(640UL, value, 0);
                }

                public double SpindleTemp
                {
                    get => this.ReadDataDouble(704UL, 0);
                    set => this.WriteData(704UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbdf1b08f6d2dd6f4UL)]
        public class FeedoptSample : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbdf1b08f6d2dd6f4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                X = reader.X;
                Y = reader.Y;
                Z = reader.Z;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.X = X;
                writer.Y = Y;
                writer.Z = Z;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double X
            {
                get;
                set;
            }

            public double Y
            {
                get;
                set;
            }

            public double Z
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double X => ctx.ReadDataDouble(0UL, 0);
                public double Y => ctx.ReadDataDouble(64UL, 0);
                public double Z => ctx.ReadDataDouble(128UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(3, 0);
                }

                public double X
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Y
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Z
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa7467c11497eeb98UL)]
        public class Position : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa7467c11497eeb98UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                X = reader.X;
                Y = reader.Y;
                Z = reader.Z;
                B = reader.B;
                C = reader.C;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.X = X;
                writer.Y = Y;
                writer.Z = Z;
                writer.B = B;
                writer.C = C;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double X
            {
                get;
                set;
            }

            public double Y
            {
                get;
                set;
            }

            public double Z
            {
                get;
                set;
            }

            public double B
            {
                get;
                set;
            }

            public double C
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double X => ctx.ReadDataDouble(0UL, 0);
                public double Y => ctx.ReadDataDouble(64UL, 0);
                public double Z => ctx.ReadDataDouble(128UL, 0);
                public double B => ctx.ReadDataDouble(192UL, 0);
                public double C => ctx.ReadDataDouble(256UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(5, 0);
                }

                public double X
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Y
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Z
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double B
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public double C
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8f8f0e1837938abUL)]
        public class AxisMode : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa8f8f0e1837938abUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Inactive = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMask>(reader.Inactive);
                Fault = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMask>(reader.Fault);
                Homing = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMask>(reader.Homing);
                Csp = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMask>(reader.Csp);
                Csv = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.AxisMask>(reader.Csv);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Inactive?.serialize(writer.Inactive);
                Fault?.serialize(writer.Fault);
                Homing?.serialize(writer.Homing);
                Csp?.serialize(writer.Csp);
                Csv?.serialize(writer.Csv);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.AxisMask Inactive
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.AxisMask Fault
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.AxisMask Homing
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.AxisMask Csp
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.AxisMask Csv
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.OpenCNServerInterface.AxisMask.READER Inactive => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.AxisMask.READER.create);
                public CapnpGen.OpenCNServerInterface.AxisMask.READER Fault => ctx.ReadStruct(1, CapnpGen.OpenCNServerInterface.AxisMask.READER.create);
                public CapnpGen.OpenCNServerInterface.AxisMask.READER Homing => ctx.ReadStruct(2, CapnpGen.OpenCNServerInterface.AxisMask.READER.create);
                public CapnpGen.OpenCNServerInterface.AxisMask.READER Csp => ctx.ReadStruct(3, CapnpGen.OpenCNServerInterface.AxisMask.READER.create);
                public CapnpGen.OpenCNServerInterface.AxisMask.READER Csv => ctx.ReadStruct(4, CapnpGen.OpenCNServerInterface.AxisMask.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 5);
                }

                public CapnpGen.OpenCNServerInterface.AxisMask.WRITER Inactive
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMask.WRITER>(0);
                    set => Link(0, value);
                }

                public CapnpGen.OpenCNServerInterface.AxisMask.WRITER Fault
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMask.WRITER>(1);
                    set => Link(1, value);
                }

                public CapnpGen.OpenCNServerInterface.AxisMask.WRITER Homing
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMask.WRITER>(2);
                    set => Link(2, value);
                }

                public CapnpGen.OpenCNServerInterface.AxisMask.WRITER Csp
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMask.WRITER>(3);
                    set => Link(3, value);
                }

                public CapnpGen.OpenCNServerInterface.AxisMask.WRITER Csv
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.AxisMask.WRITER>(4);
                    set => Link(4, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfddd767e2bb4e523UL)]
        public class AxisMask : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfddd767e2bb4e523UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                X = reader.X;
                Y = reader.Y;
                Z = reader.Z;
                B = reader.B;
                C = reader.C;
                Spindle = reader.Spindle;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.X = X;
                writer.Y = Y;
                writer.Z = Z;
                writer.B = B;
                writer.C = C;
                writer.Spindle = Spindle;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool X
            {
                get;
                set;
            }

            public bool Y
            {
                get;
                set;
            }

            public bool Z
            {
                get;
                set;
            }

            public bool B
            {
                get;
                set;
            }

            public bool C
            {
                get;
                set;
            }

            public bool Spindle
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool X => ctx.ReadDataBool(0UL, false);
                public bool Y => ctx.ReadDataBool(1UL, false);
                public bool Z => ctx.ReadDataBool(2UL, false);
                public bool B => ctx.ReadDataBool(3UL, false);
                public bool C => ctx.ReadDataBool(4UL, false);
                public bool Spindle => ctx.ReadDataBool(5UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool X
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }

                public bool Y
                {
                    get => this.ReadDataBool(1UL, false);
                    set => this.WriteData(1UL, value, false);
                }

                public bool Z
                {
                    get => this.ReadDataBool(2UL, false);
                    set => this.WriteData(2UL, value, false);
                }

                public bool B
                {
                    get => this.ReadDataBool(3UL, false);
                    set => this.WriteData(3UL, value, false);
                }

                public bool C
                {
                    get => this.ReadDataBool(4UL, false);
                    set => this.WriteData(4UL, value, false);
                }

                public bool Spindle
                {
                    get => this.ReadDataBool(5UL, false);
                    set => this.WriteData(5UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd6a2c23cd0b3212fUL)]
        public class MachineMode : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd6a2c23cd0b3212fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Homing = reader.Homing;
                Stream = reader.Stream;
                Jog = reader.Jog;
                Inactive = reader.Inactive;
                Gcode = reader.Gcode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Homing = Homing;
                writer.Stream = Stream;
                writer.Jog = Jog;
                writer.Inactive = Inactive;
                writer.Gcode = Gcode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Homing
            {
                get;
                set;
            }

            public bool Stream
            {
                get;
                set;
            }

            public bool Jog
            {
                get;
                set;
            }

            public bool Inactive
            {
                get;
                set;
            }

            public bool Gcode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Homing => ctx.ReadDataBool(0UL, false);
                public bool Stream => ctx.ReadDataBool(1UL, false);
                public bool Jog => ctx.ReadDataBool(2UL, false);
                public bool Inactive => ctx.ReadDataBool(3UL, false);
                public bool Gcode => ctx.ReadDataBool(4UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Homing
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }

                public bool Stream
                {
                    get => this.ReadDataBool(1UL, false);
                    set => this.WriteData(1UL, value, false);
                }

                public bool Jog
                {
                    get => this.ReadDataBool(2UL, false);
                    set => this.WriteData(2UL, value, false);
                }

                public bool Inactive
                {
                    get => this.ReadDataBool(3UL, false);
                    set => this.WriteData(3UL, value, false);
                }

                public bool Gcode
                {
                    get => this.ReadDataBool(4UL, false);
                    set => this.WriteData(4UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcfc1a612de4bd083UL)]
        public class Progress : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcfc1a612de4bd083UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                CompressingProgress = reader.CompressingProgress;
                CompressingCount = reader.CompressingCount;
                SmoothingProgress = reader.SmoothingProgress;
                SmoothingCount = reader.SmoothingCount;
                SplittingProgress = reader.SplittingProgress;
                SplittingCount = reader.SplittingCount;
                OptimisingProgress = reader.OptimisingProgress;
                OptimisingCount = reader.OptimisingCount;
                ResamplingProgress = reader.ResamplingProgress;
                ResamplingCount = reader.ResamplingCount;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.CompressingProgress = CompressingProgress;
                writer.CompressingCount = CompressingCount;
                writer.SmoothingProgress = SmoothingProgress;
                writer.SmoothingCount = SmoothingCount;
                writer.SplittingProgress = SplittingProgress;
                writer.SplittingCount = SplittingCount;
                writer.OptimisingProgress = OptimisingProgress;
                writer.OptimisingCount = OptimisingCount;
                writer.ResamplingProgress = ResamplingProgress;
                writer.ResamplingCount = ResamplingCount;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int CompressingProgress
            {
                get;
                set;
            }

            public int CompressingCount
            {
                get;
                set;
            }

            public int SmoothingProgress
            {
                get;
                set;
            }

            public int SmoothingCount
            {
                get;
                set;
            }

            public int SplittingProgress
            {
                get;
                set;
            }

            public int SplittingCount
            {
                get;
                set;
            }

            public int OptimisingProgress
            {
                get;
                set;
            }

            public int OptimisingCount
            {
                get;
                set;
            }

            public int ResamplingProgress
            {
                get;
                set;
            }

            public int ResamplingCount
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int CompressingProgress => ctx.ReadDataInt(0UL, 0);
                public int CompressingCount => ctx.ReadDataInt(32UL, 0);
                public int SmoothingProgress => ctx.ReadDataInt(64UL, 0);
                public int SmoothingCount => ctx.ReadDataInt(96UL, 0);
                public int SplittingProgress => ctx.ReadDataInt(128UL, 0);
                public int SplittingCount => ctx.ReadDataInt(160UL, 0);
                public int OptimisingProgress => ctx.ReadDataInt(192UL, 0);
                public int OptimisingCount => ctx.ReadDataInt(224UL, 0);
                public int ResamplingProgress => ctx.ReadDataInt(256UL, 0);
                public int ResamplingCount => ctx.ReadDataInt(288UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(5, 0);
                }

                public int CompressingProgress
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public int CompressingCount
                {
                    get => this.ReadDataInt(32UL, 0);
                    set => this.WriteData(32UL, value, 0);
                }

                public int SmoothingProgress
                {
                    get => this.ReadDataInt(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public int SmoothingCount
                {
                    get => this.ReadDataInt(96UL, 0);
                    set => this.WriteData(96UL, value, 0);
                }

                public int SplittingProgress
                {
                    get => this.ReadDataInt(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public int SplittingCount
                {
                    get => this.ReadDataInt(160UL, 0);
                    set => this.WriteData(160UL, value, 0);
                }

                public int OptimisingProgress
                {
                    get => this.ReadDataInt(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public int OptimisingCount
                {
                    get => this.ReadDataInt(224UL, 0);
                    set => this.WriteData(224UL, value, 0);
                }

                public int ResamplingProgress
                {
                    get => this.ReadDataInt(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public int ResamplingCount
                {
                    get => this.ReadDataInt(288UL, 0);
                    set => this.WriteData(288UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcd8b9ad1ae4b6395UL)]
        public class FeedOptCfg : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcd8b9ad1ae4b6395UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                NHorz = reader.NHorz;
                NDiscr = reader.NDiscr;
                NBreak = reader.NBreak;
                LSplit = reader.LSplit;
                CutOff = reader.CutOff;
                DebugPrint = reader.DebugPrint;
                Source = reader.Source;
                VmaxX = reader.VmaxX;
                VmaxY = reader.VmaxY;
                VmaxZ = reader.VmaxZ;
                VmaxA = reader.VmaxA;
                VmaxB = reader.VmaxB;
                VmaxC = reader.VmaxC;
                AmaxX = reader.AmaxX;
                AmaxY = reader.AmaxY;
                AmaxZ = reader.AmaxZ;
                AmaxA = reader.AmaxA;
                AmaxB = reader.AmaxB;
                AmaxC = reader.AmaxC;
                JmaxX = reader.JmaxX;
                JmaxY = reader.JmaxY;
                JmaxZ = reader.JmaxZ;
                JmaxA = reader.JmaxA;
                JmaxB = reader.JmaxB;
                JmaxC = reader.JmaxC;
                Vmax = reader.Vmax;
                SplitSpecialSpine = reader.SplitSpecialSpine;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.NHorz = NHorz;
                writer.NDiscr = NDiscr;
                writer.NBreak = NBreak;
                writer.LSplit = LSplit;
                writer.CutOff = CutOff;
                writer.DebugPrint = DebugPrint;
                writer.Source = Source;
                writer.VmaxX = VmaxX;
                writer.VmaxY = VmaxY;
                writer.VmaxZ = VmaxZ;
                writer.VmaxA = VmaxA;
                writer.VmaxB = VmaxB;
                writer.VmaxC = VmaxC;
                writer.AmaxX = AmaxX;
                writer.AmaxY = AmaxY;
                writer.AmaxZ = AmaxZ;
                writer.AmaxA = AmaxA;
                writer.AmaxB = AmaxB;
                writer.AmaxC = AmaxC;
                writer.JmaxX = JmaxX;
                writer.JmaxY = JmaxY;
                writer.JmaxZ = JmaxZ;
                writer.JmaxA = JmaxA;
                writer.JmaxB = JmaxB;
                writer.JmaxC = JmaxC;
                writer.Vmax = Vmax;
                writer.SplitSpecialSpine = SplitSpecialSpine;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int NHorz
            {
                get;
                set;
            }

            public int NDiscr
            {
                get;
                set;
            }

            public int NBreak
            {
                get;
                set;
            }

            public double LSplit
            {
                get;
                set;
            }

            public double CutOff
            {
                get;
                set;
            }

            public bool DebugPrint
            {
                get;
                set;
            }

            public string Source
            {
                get;
                set;
            }

            public double VmaxX
            {
                get;
                set;
            }

            public double VmaxY
            {
                get;
                set;
            }

            public double VmaxZ
            {
                get;
                set;
            }

            public double VmaxA
            {
                get;
                set;
            }

            public double VmaxB
            {
                get;
                set;
            }

            public double VmaxC
            {
                get;
                set;
            }

            public double AmaxX
            {
                get;
                set;
            }

            public double AmaxY
            {
                get;
                set;
            }

            public double AmaxZ
            {
                get;
                set;
            }

            public double AmaxA
            {
                get;
                set;
            }

            public double AmaxB
            {
                get;
                set;
            }

            public double AmaxC
            {
                get;
                set;
            }

            public double JmaxX
            {
                get;
                set;
            }

            public double JmaxY
            {
                get;
                set;
            }

            public double JmaxZ
            {
                get;
                set;
            }

            public double JmaxA
            {
                get;
                set;
            }

            public double JmaxB
            {
                get;
                set;
            }

            public double JmaxC
            {
                get;
                set;
            }

            public double Vmax
            {
                get;
                set;
            }

            public bool SplitSpecialSpine
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int NHorz => ctx.ReadDataInt(0UL, 0);
                public int NDiscr => ctx.ReadDataInt(32UL, 0);
                public int NBreak => ctx.ReadDataInt(64UL, 0);
                public double LSplit => ctx.ReadDataDouble(128UL, 0);
                public double CutOff => ctx.ReadDataDouble(192UL, 0);
                public bool DebugPrint => ctx.ReadDataBool(96UL, false);
                public string Source => ctx.ReadText(0, null);
                public double VmaxX => ctx.ReadDataDouble(256UL, 0);
                public double VmaxY => ctx.ReadDataDouble(320UL, 0);
                public double VmaxZ => ctx.ReadDataDouble(384UL, 0);
                public double VmaxA => ctx.ReadDataDouble(448UL, 0);
                public double VmaxB => ctx.ReadDataDouble(512UL, 0);
                public double VmaxC => ctx.ReadDataDouble(576UL, 0);
                public double AmaxX => ctx.ReadDataDouble(640UL, 0);
                public double AmaxY => ctx.ReadDataDouble(704UL, 0);
                public double AmaxZ => ctx.ReadDataDouble(768UL, 0);
                public double AmaxA => ctx.ReadDataDouble(832UL, 0);
                public double AmaxB => ctx.ReadDataDouble(896UL, 0);
                public double AmaxC => ctx.ReadDataDouble(960UL, 0);
                public double JmaxX => ctx.ReadDataDouble(1024UL, 0);
                public double JmaxY => ctx.ReadDataDouble(1088UL, 0);
                public double JmaxZ => ctx.ReadDataDouble(1152UL, 0);
                public double JmaxA => ctx.ReadDataDouble(1216UL, 0);
                public double JmaxB => ctx.ReadDataDouble(1280UL, 0);
                public double JmaxC => ctx.ReadDataDouble(1344UL, 0);
                public double Vmax => ctx.ReadDataDouble(1408UL, 0);
                public bool SplitSpecialSpine => ctx.ReadDataBool(97UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(23, 1);
                }

                public int NHorz
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public int NDiscr
                {
                    get => this.ReadDataInt(32UL, 0);
                    set => this.WriteData(32UL, value, 0);
                }

                public int NBreak
                {
                    get => this.ReadDataInt(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double LSplit
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double CutOff
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }

                public bool DebugPrint
                {
                    get => this.ReadDataBool(96UL, false);
                    set => this.WriteData(96UL, value, false);
                }

                public string Source
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public double VmaxX
                {
                    get => this.ReadDataDouble(256UL, 0);
                    set => this.WriteData(256UL, value, 0);
                }

                public double VmaxY
                {
                    get => this.ReadDataDouble(320UL, 0);
                    set => this.WriteData(320UL, value, 0);
                }

                public double VmaxZ
                {
                    get => this.ReadDataDouble(384UL, 0);
                    set => this.WriteData(384UL, value, 0);
                }

                public double VmaxA
                {
                    get => this.ReadDataDouble(448UL, 0);
                    set => this.WriteData(448UL, value, 0);
                }

                public double VmaxB
                {
                    get => this.ReadDataDouble(512UL, 0);
                    set => this.WriteData(512UL, value, 0);
                }

                public double VmaxC
                {
                    get => this.ReadDataDouble(576UL, 0);
                    set => this.WriteData(576UL, value, 0);
                }

                public double AmaxX
                {
                    get => this.ReadDataDouble(640UL, 0);
                    set => this.WriteData(640UL, value, 0);
                }

                public double AmaxY
                {
                    get => this.ReadDataDouble(704UL, 0);
                    set => this.WriteData(704UL, value, 0);
                }

                public double AmaxZ
                {
                    get => this.ReadDataDouble(768UL, 0);
                    set => this.WriteData(768UL, value, 0);
                }

                public double AmaxA
                {
                    get => this.ReadDataDouble(832UL, 0);
                    set => this.WriteData(832UL, value, 0);
                }

                public double AmaxB
                {
                    get => this.ReadDataDouble(896UL, 0);
                    set => this.WriteData(896UL, value, 0);
                }

                public double AmaxC
                {
                    get => this.ReadDataDouble(960UL, 0);
                    set => this.WriteData(960UL, value, 0);
                }

                public double JmaxX
                {
                    get => this.ReadDataDouble(1024UL, 0);
                    set => this.WriteData(1024UL, value, 0);
                }

                public double JmaxY
                {
                    get => this.ReadDataDouble(1088UL, 0);
                    set => this.WriteData(1088UL, value, 0);
                }

                public double JmaxZ
                {
                    get => this.ReadDataDouble(1152UL, 0);
                    set => this.WriteData(1152UL, value, 0);
                }

                public double JmaxA
                {
                    get => this.ReadDataDouble(1216UL, 0);
                    set => this.WriteData(1216UL, value, 0);
                }

                public double JmaxB
                {
                    get => this.ReadDataDouble(1280UL, 0);
                    set => this.WriteData(1280UL, value, 0);
                }

                public double JmaxC
                {
                    get => this.ReadDataDouble(1344UL, 0);
                    set => this.WriteData(1344UL, value, 0);
                }

                public double Vmax
                {
                    get => this.ReadDataDouble(1408UL, 0);
                    set => this.WriteData(1408UL, value, 0);
                }

                public bool SplitSpecialSpine
                {
                    get => this.ReadDataBool(97UL, false);
                    set => this.WriteData(97UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe60df46347b664ccUL)]
        public class PinValue : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe60df46347b664ccUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.PinValue.value>(reader.Value);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Value?.serialize(writer.Value);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.PinValue.value Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public value.READER Value => new value.READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public value.WRITER Value
                {
                    get => Rewrap<value.WRITER>();
                }
            }

            [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfbfdd2f9008c7cc1UL)]
            public class value : ICapnpSerializable
            {
                public const UInt64 typeId = 0xfbfdd2f9008c7cc1UL;
                public enum WHICH : ushort
                {
                    U = 0,
                    S = 1,
                    F = 2,
                    B = 3,
                    undefined = 65535
                }

                void ICapnpSerializable.Deserialize(DeserializerState arg_)
                {
                    var reader = READER.create(arg_);
                    switch (reader.which)
                    {
                        case WHICH.U:
                            U = reader.U;
                            break;
                        case WHICH.S:
                            S = reader.S;
                            break;
                        case WHICH.F:
                            F = reader.F;
                            break;
                        case WHICH.B:
                            B = reader.B;
                            break;
                    }

                    applyDefaults();
                }

                private WHICH _which = WHICH.undefined;
                private object _content;
                public WHICH which
                {
                    get => _which;
                    set
                    {
                        if (value == _which)
                            return;
                        _which = value;
                        switch (value)
                        {
                            case WHICH.U:
                                _content = 0;
                                break;
                            case WHICH.S:
                                _content = 0;
                                break;
                            case WHICH.F:
                                _content = 0;
                                break;
                            case WHICH.B:
                                _content = false;
                                break;
                        }
                    }
                }

                public void serialize(WRITER writer)
                {
                    writer.which = which;
                    switch (which)
                    {
                        case WHICH.U:
                            writer.U = U.Value;
                            break;
                        case WHICH.S:
                            writer.S = S.Value;
                            break;
                        case WHICH.F:
                            writer.F = F.Value;
                            break;
                        case WHICH.B:
                            writer.B = B.Value;
                            break;
                    }
                }

                void ICapnpSerializable.Serialize(SerializerState arg_)
                {
                    serialize(arg_.Rewrap<WRITER>());
                }

                public void applyDefaults()
                {
                }

                public uint? U
                {
                    get => _which == WHICH.U ? (uint? )_content : null;
                    set
                    {
                        _which = WHICH.U;
                        _content = value;
                    }
                }

                public int? S
                {
                    get => _which == WHICH.S ? (int? )_content : null;
                    set
                    {
                        _which = WHICH.S;
                        _content = value;
                    }
                }

                public double? F
                {
                    get => _which == WHICH.F ? (double? )_content : null;
                    set
                    {
                        _which = WHICH.F;
                        _content = value;
                    }
                }

                public bool? B
                {
                    get => _which == WHICH.B ? (bool? )_content : null;
                    set
                    {
                        _which = WHICH.B;
                        _content = value;
                    }
                }

                public struct READER
                {
                    readonly DeserializerState ctx;
                    public READER(DeserializerState ctx)
                    {
                        this.ctx = ctx;
                    }

                    public static READER create(DeserializerState ctx) => new READER(ctx);
                    public static implicit operator DeserializerState(READER reader) => reader.ctx;
                    public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                    public WHICH which => (WHICH)ctx.ReadDataUShort(32U, (ushort)0);
                    public uint U => which == WHICH.U ? ctx.ReadDataUInt(0UL, 0U) : default;
                    public int S => which == WHICH.S ? ctx.ReadDataInt(0UL, 0) : default;
                    public double F => which == WHICH.F ? ctx.ReadDataDouble(64UL, 0) : default;
                    public bool B => which == WHICH.B ? ctx.ReadDataBool(0UL, false) : default;
                }

                public class WRITER : SerializerState
                {
                    public WRITER()
                    {
                    }

                    public WHICH which
                    {
                        get => (WHICH)this.ReadDataUShort(32U, (ushort)0);
                        set => this.WriteData(32U, (ushort)value, (ushort)0);
                    }

                    public uint U
                    {
                        get => which == WHICH.U ? this.ReadDataUInt(0UL, 0U) : default;
                        set => this.WriteData(0UL, value, 0U);
                    }

                    public int S
                    {
                        get => which == WHICH.S ? this.ReadDataInt(0UL, 0) : default;
                        set => this.WriteData(0UL, value, 0);
                    }

                    public double F
                    {
                        get => which == WHICH.F ? this.ReadDataDouble(64UL, 0) : default;
                        set => this.WriteData(64UL, value, 0);
                    }

                    public bool B
                    {
                        get => which == WHICH.B ? this.ReadDataBool(0UL, false) : default;
                        set => this.WriteData(0UL, value, false);
                    }
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa8b24cbb662416b1UL)]
        public class Sample : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa8b24cbb662416b1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Values = reader.Values?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.PinValue>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Values.Init(Values, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.OpenCNServerInterface.PinValue> Values
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.OpenCNServerInterface.PinValue.READER> Values => ctx.ReadList(0).Cast(CapnpGen.OpenCNServerInterface.PinValue.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.OpenCNServerInterface.PinValue.WRITER> Values
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.OpenCNServerInterface.PinValue.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfddad509ef0256d5UL)]
        public class Limit : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfddad509ef0256d5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Min = reader.Min;
                Max = reader.Max;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Min = Min;
                writer.Max = Max;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Min
            {
                get;
                set;
            }

            public double Max
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Min => ctx.ReadDataDouble(0UL, 0);
                public double Max => ctx.ReadDataDouble(64UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public double Min
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Max
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x94d9c6f1e746180cUL)]
        public class Params_Dummy0 : ICapnpSerializable
        {
            public const UInt64 typeId = 0x94d9c6f1e746180cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd00f3a9205281d84UL)]
        public class Result_Dummy0 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd00f3a9205281d84UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba7088ad1cf2e7e3UL)]
        public class Params_Dummy1 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xba7088ad1cf2e7e3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc70d2c00a6218156UL)]
        public class Result_Dummy1 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc70d2c00a6218156UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd56edd726b4a4dbeUL)]
        public class Params_SetFeedoptCommitCfg : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd56edd726b4a4dbeUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Commit = reader.Commit;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Commit = Commit;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Commit
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Commit => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Commit
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc94b0171f8b0a944UL)]
        public class Result_SetFeedoptCommitCfg : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc94b0171f8b0a944UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaaa9d7eddf64fc23UL)]
        public class Params_GetCyclicData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaaa9d7eddf64fc23UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc634f53eb33737e4UL)]
        public class Result_GetCyclicData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc634f53eb33737e4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.CyclicData>(reader.Data);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Data?.serialize(writer.Data);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.CyclicData Data
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.OpenCNServerInterface.CyclicData.READER Data => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.CyclicData.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.OpenCNServerInterface.CyclicData.WRITER Data
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.CyclicData.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf3c17cd4ff9e10e2UL)]
        public class Params_SetLcctSetMachineModeHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf3c17cd4ff9e10e2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xabf45e29bcfe3623UL)]
        public class Result_SetLcctSetMachineModeHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0xabf45e29bcfe3623UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfb216604f0268a50UL)]
        public class Params_SetLcctSetMachineModeStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfb216604f0268a50UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe946aeeb03ff1acUL)]
        public class Result_SetLcctSetMachineModeStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfe946aeeb03ff1acUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd85936d95ce2f392UL)]
        public class Params_SetLcctSetMachineModeJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd85936d95ce2f392UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x83fce31a2394b63aUL)]
        public class Result_SetLcctSetMachineModeJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x83fce31a2394b63aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x908902dae37848d1UL)]
        public class Params_SetLcctSetMachineModeInactive : ICapnpSerializable
        {
            public const UInt64 typeId = 0x908902dae37848d1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf8dd223f2086ffddUL)]
        public class Result_SetLcctSetMachineModeInactive : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf8dd223f2086ffddUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8a23db1c0f588cf7UL)]
        public class Params_SetLcctSetMachineModeGCode : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8a23db1c0f588cf7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xca262677e14358aaUL)]
        public class Result_SetLcctSetMachineModeGCode : ICapnpSerializable
        {
            public const UInt64 typeId = 0xca262677e14358aaUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d87c288b18598e0UL)]
        public class Params_Dummy9 : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d87c288b18598e0UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x84c00eb748ee880eUL)]
        public class Result_Dummy9 : ICapnpSerializable
        {
            public const UInt64 typeId = 0x84c00eb748ee880eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9410db5c1b95898cUL)]
        public class Params_SetStartHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9410db5c1b95898cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd502dcd61df3dd43UL)]
        public class Result_SetStartHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd502dcd61df3dd43UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba0cb72755ad0c81UL)]
        public class Params_SetStopHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0xba0cb72755ad0c81UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9adb39571ce7b7c1UL)]
        public class Result_SetStopHoming : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9adb39571ce7b7c1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9d3d82c6fe8e2743UL)]
        public class Params_SetHomePositionX : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9d3d82c6fe8e2743UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Position = reader.Position;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Position = Position;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Position
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Position => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Position
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xafe265ff0acba912UL)]
        public class Result_SetHomePositionX : ICapnpSerializable
        {
            public const UInt64 typeId = 0xafe265ff0acba912UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd17bbf55f4eb616fUL)]
        public class Params_SetHomePositionY : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd17bbf55f4eb616fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Position = reader.Position;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Position = Position;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Position
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Position => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Position
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdf02092e53cabd57UL)]
        public class Result_SetHomePositionY : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdf02092e53cabd57UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9ad023ab50197d6eUL)]
        public class Params_SetHomePositionZ : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9ad023ab50197d6eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Position = reader.Position;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Position = Position;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Position
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Position => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Position
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe587cad4461f3f20UL)]
        public class Result_SetHomePositionZ : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe587cad4461f3f20UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x81e5ce6bda07ccc6UL)]
        public class Params_SetSpeedSpindle : ICapnpSerializable
        {
            public const UInt64 typeId = 0x81e5ce6bda07ccc6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Speed = reader.Speed;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Speed = Speed;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Speed
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Speed => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Speed
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfe8ea4c33ad9dc55UL)]
        public class Result_SetSpeedSpindle : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfe8ea4c33ad9dc55UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9c69f7d947429388UL)]
        public class Params_SetActiveSpindle : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9c69f7d947429388UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc9e46d43ae680caeUL)]
        public class Result_SetActiveSpindle : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc9e46d43ae680caeUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xccf47e31c4da8a53UL)]
        public class Params_SetJogX : ICapnpSerializable
        {
            public const UInt64 typeId = 0xccf47e31c4da8a53UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xded6a6c9e11ab247UL)]
        public class Result_SetJogX : ICapnpSerializable
        {
            public const UInt64 typeId = 0xded6a6c9e11ab247UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda44714f93d5e7ccUL)]
        public class Params_SetJogY : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda44714f93d5e7ccUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd79ccd96033f25c3UL)]
        public class Result_SetJogY : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd79ccd96033f25c3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xedc6c31fb96d9511UL)]
        public class Params_SetJogZ : ICapnpSerializable
        {
            public const UInt64 typeId = 0xedc6c31fb96d9511UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xba32fbba8fb76a7fUL)]
        public class Result_SetJogZ : ICapnpSerializable
        {
            public const UInt64 typeId = 0xba32fbba8fb76a7fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x888ce3fe57eecc85UL)]
        public class Params_SetRelJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x888ce3fe57eecc85UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Value => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Value
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9589a9cfdad470e2UL)]
        public class Result_SetRelJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9589a9cfdad470e2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa0e5438346a3f46fUL)]
        public class Params_SetPlusJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa0e5438346a3f46fUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Plus = reader.Plus;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Plus = Plus;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Plus
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Plus => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Plus
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd5d5bcf92b1bb768UL)]
        public class Result_SetPlusJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd5d5bcf92b1bb768UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe5c4a931d1738cb7UL)]
        public class Params_SetMinusJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe5c4a931d1738cb7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Minus = reader.Minus;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Minus = Minus;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Minus
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Minus => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Minus
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe34cda0f65dba83eUL)]
        public class Result_SetMinusJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe34cda0f65dba83eUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x97dc5760625ccb92UL)]
        public class Params_SetAbsJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x97dc5760625ccb92UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Value = reader.Value;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Value = Value;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Value
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Value => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Value
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa60102323d6eae52UL)]
        public class Result_SetAbsJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa60102323d6eae52UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8ea40be178b595d4UL)]
        public class Params_SetGoJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8ea40be178b595d4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbd4c35c139db288aUL)]
        public class Result_SetGoJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbd4c35c139db288aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x86f5f7fe5f0ab53bUL)]
        public class Params_SetSpeedJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x86f5f7fe5f0ab53bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Speed = reader.Speed;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Speed = Speed;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Speed
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Speed => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Speed
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xae38be83698a93f2UL)]
        public class Result_SetSpeedJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xae38be83698a93f2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcc4bda9e7d40b580UL)]
        public class Params_SetStopJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcc4bda9e7d40b580UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xaf88235abe204f05UL)]
        public class Result_SetStopJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xaf88235abe204f05UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa91d9243ccdfb391UL)]
        public class Params_SetOffset : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa91d9243ccdfb391UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                X = reader.X;
                Y = reader.Y;
                Z = reader.Z;
                C = reader.C;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.X = X;
                writer.Y = Y;
                writer.Z = Z;
                writer.C = C;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double X
            {
                get;
                set;
            }

            public double Y
            {
                get;
                set;
            }

            public double Z
            {
                get;
                set;
            }

            public double C
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double X => ctx.ReadDataDouble(0UL, 0);
                public double Y => ctx.ReadDataDouble(64UL, 0);
                public double Z => ctx.ReadDataDouble(128UL, 0);
                public double C => ctx.ReadDataDouble(192UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(4, 0);
                }

                public double X
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }

                public double Y
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public double Z
                {
                    get => this.ReadDataDouble(128UL, 0);
                    set => this.WriteData(128UL, value, 0);
                }

                public double C
                {
                    get => this.ReadDataDouble(192UL, 0);
                    set => this.WriteData(192UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x88cae412b425b32aUL)]
        public class Result_SetOffset : ICapnpSerializable
        {
            public const UInt64 typeId = 0x88cae412b425b32aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb45f1c41e8ebf5baUL)]
        public class Params_Dummy28 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb45f1c41e8ebf5baUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfc687e5e41a7d110UL)]
        public class Result_Dummy28 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfc687e5e41a7d110UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc5d464b4e8931a22UL)]
        public class Params_Dummy29 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc5d464b4e8931a22UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdb5492da2947fcfbUL)]
        public class Result_Dummy29 : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdb5492da2947fcfbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc92d9c0dda66d757UL)]
        public class Params_SetStartStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc92d9c0dda66d757UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd16a455a6976b033UL)]
        public class Result_SetStartStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd16a455a6976b033UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbc6e924009f9269bUL)]
        public class Params_SetStopStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbc6e924009f9269bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf9fe19a15771cf26UL)]
        public class Result_SetStopStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf9fe19a15771cf26UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb980d2fed3b6fc3bUL)]
        public class Params_SetGcodeStart : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb980d2fed3b6fc3bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe2dd01349affe790UL)]
        public class Result_SetGcodeStart : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe2dd01349affe790UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcbaf279d6d698135UL)]
        public class Params_SetGcodePause : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcbaf279d6d698135UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4ff2ed5aa4201faUL)]
        public class Result_SetGcodePause : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb4ff2ed5aa4201faUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd84c2e81be3e94ffUL)]
        public class Params_SetFaultReset : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd84c2e81be3e94ffUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Reset = reader.Reset;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Reset = Reset;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Reset
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Reset => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Reset
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa861c31ce893bce4UL)]
        public class Result_SetFaultReset : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa861c31ce893bce4UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xf2317ca3259db1ffUL)]
        public class Params_SetFeedrateScale : ICapnpSerializable
        {
            public const UInt64 typeId = 0xf2317ca3259db1ffUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Scale = reader.Scale;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Scale = Scale;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Scale
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Scale => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Scale
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfc0dc5bdb3e9c998UL)]
        public class Result_SetFeedrateScale : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfc0dc5bdb3e9c998UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x9cca4ce002443ab6UL)]
        public class Params_SetFeedoptReset : ICapnpSerializable
        {
            public const UInt64 typeId = 0x9cca4ce002443ab6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Reset = reader.Reset;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Reset = Reset;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Reset
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Reset => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Reset
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4077764f11210e7UL)]
        public class Result_SetFeedoptReset : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe4077764f11210e7UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x94b7d45e1b999256UL)]
        public class Params_ReadLog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x94b7d45e1b999256UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd085ec11e5b9e1e5UL)]
        public class Result_ReadLog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd085ec11e5b9e1e5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Message = reader.Message;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Message = Message;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Message
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Message => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Message
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdfcdf2a946a77eb8UL)]
        public class Params_SetFeedoptConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdfcdf2a946a77eb8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Config = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.FeedOptCfg>(reader.Config);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Config?.serialize(writer.Config);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.FeedOptCfg Config
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.OpenCNServerInterface.FeedOptCfg.READER Config => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.FeedOptCfg.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.OpenCNServerInterface.FeedOptCfg.WRITER Config
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.FeedOptCfg.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc83209b0bc17dd4dUL)]
        public class Result_SetFeedoptConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc83209b0bc17dd4dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe1e62d654c30890dUL)]
        public class Params_GetFeedoptConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe1e62d654c30890dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xece13f8a02fb3d30UL)]
        public class Result_GetFeedoptConfig : ICapnpSerializable
        {
            public const UInt64 typeId = 0xece13f8a02fb3d30UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Config = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.FeedOptCfg>(reader.Config);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                Config?.serialize(writer.Config);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.FeedOptCfg Config
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.OpenCNServerInterface.FeedOptCfg.READER Config => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.FeedOptCfg.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public CapnpGen.OpenCNServerInterface.FeedOptCfg.WRITER Config
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.FeedOptCfg.WRITER>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x92d81588aacd14e5UL)]
        public class Params_ToolpathStartChannel : ICapnpSerializable
        {
            public const UInt64 typeId = 0x92d81588aacd14e5UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                SampleRate = reader.SampleRate;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.SampleRate = SampleRate;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int SampleRate
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int SampleRate => ctx.ReadDataInt(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public int SampleRate
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa869229536969b05UL)]
        public class Result_ToolpathStartChannel : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa869229536969b05UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Result => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Result
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x852439e79ffc5863UL)]
        public class Params_ToolpathStopChannel : ICapnpSerializable
        {
            public const UInt64 typeId = 0x852439e79ffc5863UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xae65e26f4a15a51dUL)]
        public class Result_ToolpathStopChannel : ICapnpSerializable
        {
            public const UInt64 typeId = 0xae65e26f4a15a51dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbc4685faf01cb762UL)]
        public class Params_ToolpathReadSamples : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbc4685faf01cb762UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd68fe435cf6009e8UL)]
        public class Result_ToolpathReadSamples : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd68fe435cf6009e8UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Samples = reader.Samples?.ToReadOnlyList(_ => CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Sample>(_));
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Samples.Init(Samples, (_s1, _v1) => _v1?.serialize(_s1));
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<CapnpGen.OpenCNServerInterface.Sample> Samples
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<CapnpGen.OpenCNServerInterface.Sample.READER> Samples => ctx.ReadList(0).Cast(CapnpGen.OpenCNServerInterface.Sample.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfStructsSerializer<CapnpGen.OpenCNServerInterface.Sample.WRITER> Samples
                {
                    get => BuildPointer<ListOfStructsSerializer<CapnpGen.OpenCNServerInterface.Sample.WRITER>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbed746e703c78077UL)]
        public class Params_SendFileParam : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbed746e703c78077UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                FileName = reader.FileName;
                Size = reader.Size;
                FileOp = reader.FileOp;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.FileName = FileName;
                writer.Size = Size;
                writer.FileOp = FileOp;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string FileName
            {
                get;
                set;
            }

            public uint Size
            {
                get;
                set;
            }

            public int FileOp
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string FileName => ctx.ReadText(0, null);
                public uint Size => ctx.ReadDataUInt(0UL, 0U);
                public int FileOp => ctx.ReadDataInt(32UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 1);
                }

                public string FileName
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }

                public uint Size
                {
                    get => this.ReadDataUInt(0UL, 0U);
                    set => this.WriteData(0UL, value, 0U);
                }

                public int FileOp
                {
                    get => this.ReadDataInt(32UL, 0);
                    set => this.WriteData(32UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x99aabc9d5d261320UL)]
        public class Result_SendFileParam : ICapnpSerializable
        {
            public const UInt64 typeId = 0x99aabc9d5d261320UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int Result => ctx.ReadDataInt(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public int Result
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xdab6772ac9ee4662UL)]
        public class Params_SendFileData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xdab6772ac9ee4662UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data.Init(Data);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<byte> Data
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<byte> Data => ctx.ReadList(0).CastByte();
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPrimitivesSerializer<byte> Data
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe40c826ac6b097ebUL)]
        public class Result_SendFileData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe40c826ac6b097ebUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int Result => ctx.ReadDataInt(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public int Result
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe8e734dea677ccfeUL)]
        public class Params_PathExist : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe8e734dea677ccfeUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Path = reader.Path;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Path = Path;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string Path
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string Path => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string Path
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa39841af57d55d57UL)]
        public class Result_PathExist : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa39841af57d55d57UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Result => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Result
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbe0b100cf14c8e53UL)]
        public class Params_CreateFolder : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbe0b100cf14c8e53UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                FolderPath = reader.FolderPath;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.FolderPath = FolderPath;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public string FolderPath
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public string FolderPath => ctx.ReadText(0, null);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public string FolderPath
                {
                    get => this.ReadText(0, null);
                    set => this.WriteText(0, value, null);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xda32e2e27e4acaf1UL)]
        public class Result_CreateFolder : ICapnpSerializable
        {
            public const UInt64 typeId = 0xda32e2e27e4acaf1UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Result => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Result
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xfc4040d6adfd0ee6UL)]
        public class Params_SetLoadStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xfc4040d6adfd0ee6UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe4486e3de9181908UL)]
        public class Result_SetLoadStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe4486e3de9181908UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb347fbab06fbef19UL)]
        public class Params_SetPauseStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb347fbab06fbef19UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Mode = reader.Mode;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Mode = Mode;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Mode
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Mode => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Mode
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x860d2eaa0e36f757UL)]
        public class Result_SetPauseStream : ICapnpSerializable
        {
            public const UInt64 typeId = 0x860d2eaa0e36f757UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x97a4f475ccfbf68dUL)]
        public class Params_SamplerNewFile : ICapnpSerializable
        {
            public const UInt64 typeId = 0x97a4f475ccfbf68dUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xcb25eb18c2f01fc2UL)]
        public class Result_SamplerNewFile : ICapnpSerializable
        {
            public const UInt64 typeId = 0xcb25eb18c2f01fc2UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xab84aba81fbca268UL)]
        public class Params_GetFileData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xab84aba81fbca268UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbad7e94349be1955UL)]
        public class Result_GetFileData : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbad7e94349be1955UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Data = reader.Data;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Data.Init(Data);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public IReadOnlyList<byte> Data
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public IReadOnlyList<byte> Data => ctx.ReadList(0).CastByte();
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 1);
                }

                public ListOfPrimitivesSerializer<byte> Data
                {
                    get => BuildPointer<ListOfPrimitivesSerializer<byte>>(0);
                    set => Link(0, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xce333bca0b1ad40aUL)]
        public class Params_SetSamplerDownloadFile : ICapnpSerializable
        {
            public const UInt64 typeId = 0xce333bca0b1ad40aUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Download = reader.Download;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Download = Download;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Download
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Download => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Download
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xabf9fdc2371bda77UL)]
        public class Result_SetSamplerDownloadFile : ICapnpSerializable
        {
            public const UInt64 typeId = 0xabf9fdc2371bda77UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xac4195d24d2eb831UL)]
        public class Params_SetSpindleThreshold : ICapnpSerializable
        {
            public const UInt64 typeId = 0xac4195d24d2eb831UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Percent = reader.Percent;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Percent = Percent;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public int Percent
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public int Percent => ctx.ReadDataInt(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public int Percent
                {
                    get => this.ReadDataInt(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x88c87b6fdf59d6fbUL)]
        public class Result_SetSpindleThreshold : ICapnpSerializable
        {
            public const UInt64 typeId = 0x88c87b6fdf59d6fbUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x84439c92b8d84e71UL)]
        public class Params_StartJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0x84439c92b8d84e71UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Axis = reader.Axis;
                Target = reader.Target;
                Relative = reader.Relative;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Axis = Axis;
                writer.Target = Target;
                writer.Relative = Relative;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public byte Axis
            {
                get;
                set;
            }

            public double Target
            {
                get;
                set;
            }

            public bool Relative
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public byte Axis => ctx.ReadDataByte(0UL, (byte)0);
                public double Target => ctx.ReadDataDouble(64UL, 0);
                public bool Relative => ctx.ReadDataBool(8UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(2, 0);
                }

                public byte Axis
                {
                    get => this.ReadDataByte(0UL, (byte)0);
                    set => this.WriteData(0UL, value, (byte)0);
                }

                public double Target
                {
                    get => this.ReadDataDouble(64UL, 0);
                    set => this.WriteData(64UL, value, 0);
                }

                public bool Relative
                {
                    get => this.ReadDataBool(8UL, false);
                    set => this.WriteData(8UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd72d4b4d76d53778UL)]
        public class Result_StartJog : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd72d4b4d76d53778UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd4c7e1e86490686cUL)]
        public class Params_SetFreeTool : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd4c7e1e86490686cUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                State = reader.State;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.State = State;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool State
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool State => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool State
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb6f695b0a44ada0bUL)]
        public class Result_SetFreeTool : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb6f695b0a44ada0bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa717f1944b4b8489UL)]
        public class Params_SetFreePalette : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa717f1944b4b8489UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                State = reader.State;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.State = State;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool State
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool State => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool State
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xec342a24a0a9d421UL)]
        public class Result_SetFreePalette : ICapnpSerializable
        {
            public const UInt64 typeId = 0xec342a24a0a9d421UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xe501411ac654fd31UL)]
        public class Params_GetMachineLimits : ICapnpSerializable
        {
            public const UInt64 typeId = 0xe501411ac654fd31UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x89d99ca16f826ea9UL)]
        public class Result_GetMachineLimits : ICapnpSerializable
        {
            public const UInt64 typeId = 0x89d99ca16f826ea9UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                X = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Limit>(reader.X);
                Y = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Limit>(reader.Y);
                Z = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Limit>(reader.Z);
                B = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Limit>(reader.B);
                C = CapnpSerializable.Create<CapnpGen.OpenCNServerInterface.Limit>(reader.C);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                X?.serialize(writer.X);
                Y?.serialize(writer.Y);
                Z?.serialize(writer.Z);
                B?.serialize(writer.B);
                C?.serialize(writer.C);
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public CapnpGen.OpenCNServerInterface.Limit X
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Limit Y
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Limit Z
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Limit B
            {
                get;
                set;
            }

            public CapnpGen.OpenCNServerInterface.Limit C
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public CapnpGen.OpenCNServerInterface.Limit.READER X => ctx.ReadStruct(0, CapnpGen.OpenCNServerInterface.Limit.READER.create);
                public CapnpGen.OpenCNServerInterface.Limit.READER Y => ctx.ReadStruct(1, CapnpGen.OpenCNServerInterface.Limit.READER.create);
                public CapnpGen.OpenCNServerInterface.Limit.READER Z => ctx.ReadStruct(2, CapnpGen.OpenCNServerInterface.Limit.READER.create);
                public CapnpGen.OpenCNServerInterface.Limit.READER B => ctx.ReadStruct(3, CapnpGen.OpenCNServerInterface.Limit.READER.create);
                public CapnpGen.OpenCNServerInterface.Limit.READER C => ctx.ReadStruct(4, CapnpGen.OpenCNServerInterface.Limit.READER.create);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 5);
                }

                public CapnpGen.OpenCNServerInterface.Limit.WRITER X
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Limit.WRITER>(0);
                    set => Link(0, value);
                }

                public CapnpGen.OpenCNServerInterface.Limit.WRITER Y
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Limit.WRITER>(1);
                    set => Link(1, value);
                }

                public CapnpGen.OpenCNServerInterface.Limit.WRITER Z
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Limit.WRITER>(2);
                    set => Link(2, value);
                }

                public CapnpGen.OpenCNServerInterface.Limit.WRITER B
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Limit.WRITER>(3);
                    set => Link(3, value);
                }

                public CapnpGen.OpenCNServerInterface.Limit.WRITER C
                {
                    get => BuildPointer<CapnpGen.OpenCNServerInterface.Limit.WRITER>(4);
                    set => Link(4, value);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xb4da1b941acd1170UL)]
        public class Params_GetSpindleOverride : ICapnpSerializable
        {
            public const UInt64 typeId = 0xb4da1b941acd1170UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd60e1191b28c8a36UL)]
        public class Result_GetSpindleOverride : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd60e1191b28c8a36UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Result => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Result
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xbbf66be84d57910bUL)]
        public class Params_SetSpindleOverride : ICapnpSerializable
        {
            public const UInt64 typeId = 0xbbf66be84d57910bUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Override = reader.Override;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Override = Override;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public double Override
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public double Override => ctx.ReadDataDouble(0UL, 0);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public double Override
                {
                    get => this.ReadDataDouble(0UL, 0);
                    set => this.WriteData(0UL, value, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x8fa3d9325cba2ae3UL)]
        public class Result_SetSpindleOverride : ICapnpSerializable
        {
            public const UInt64 typeId = 0x8fa3d9325cba2ae3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xef532647900a04cdUL)]
        public class Params_GetLubStatus : ICapnpSerializable
        {
            public const UInt64 typeId = 0xef532647900a04cdUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xd31b45a22b12c921UL)]
        public class Result_GetLubStatus : ICapnpSerializable
        {
            public const UInt64 typeId = 0xd31b45a22b12c921UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                Result = reader.Result;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.Result = Result;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool Result
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool Result => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool Result
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0x85c96b23a60b7ae3UL)]
        public class Params_SetLubForce : ICapnpSerializable
        {
            public const UInt64 typeId = 0x85c96b23a60b7ae3UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LubForce = reader.LubForce;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.LubForce = LubForce;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool LubForce
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool LubForce => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool LubForce
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc9c0ba0076dac193UL)]
        public class Result_SetLubForce : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc9c0ba0076dac193UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xa5b0eae6b8eb1ffcUL)]
        public class Params_SetLubWhenMilling : ICapnpSerializable
        {
            public const UInt64 typeId = 0xa5b0eae6b8eb1ffcUL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                LubWhenMilling = reader.LubWhenMilling;
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
                writer.LubWhenMilling = LubWhenMilling;
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public bool LubWhenMilling
            {
                get;
                set;
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
                public bool LubWhenMilling => ctx.ReadDataBool(0UL, false);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(1, 0);
                }

                public bool LubWhenMilling
                {
                    get => this.ReadDataBool(0UL, false);
                    set => this.WriteData(0UL, value, false);
                }
            }
        }

        [System.CodeDom.Compiler.GeneratedCode("capnpc-csharp", "1.3.0.0"), TypeId(0xc59338d85d2cb572UL)]
        public class Result_SetLubWhenMilling : ICapnpSerializable
        {
            public const UInt64 typeId = 0xc59338d85d2cb572UL;
            void ICapnpSerializable.Deserialize(DeserializerState arg_)
            {
                var reader = READER.create(arg_);
                applyDefaults();
            }

            public void serialize(WRITER writer)
            {
            }

            void ICapnpSerializable.Serialize(SerializerState arg_)
            {
                serialize(arg_.Rewrap<WRITER>());
            }

            public void applyDefaults()
            {
            }

            public struct READER
            {
                readonly DeserializerState ctx;
                public READER(DeserializerState ctx)
                {
                    this.ctx = ctx;
                }

                public static READER create(DeserializerState ctx) => new READER(ctx);
                public static implicit operator DeserializerState(READER reader) => reader.ctx;
                public static implicit operator READER(DeserializerState ctx) => new READER(ctx);
            }

            public class WRITER : SerializerState
            {
                public WRITER()
                {
                    this.SetStruct(0, 0);
                }
            }
        }
    }
}
#pragma warning restore
