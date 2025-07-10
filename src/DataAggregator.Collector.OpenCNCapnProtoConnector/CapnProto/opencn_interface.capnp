@0x8da26520438371a4;

interface CMCtlPins {

    enum PinType {
        bit   @0;
        float @1;
        s32   @2;
        u32   @3;
        unspecified @4;
    }

    enum PinDir {
        in    @0;
        out   @1;
        inout @2;
        unspecified @3;
    }

    enum PinAccess {
        getPin @0;
        setPin @1;
    }

    struct PinValue {
        value :union {
            u @0: UInt32;
            s @1: Int32;
            f @2: Float64;
            b @3: Bool;
        }
    }

    struct Pin {
        name      @0: Text;
        type      @1: PinType;
        direction @2: PinDir;
    }

    struct PinAction {
        name   @0: Text;
        action @1: PinAccess;
        value  @2: PinValue;
    }

    struct PinResult {
        value  @0: PinValue;
        error  @1: UInt8;
    }

    getPinList @0 () -> (pins : List(Pin));

    setPin @1 (name: Text, value: PinValue) -> (error : UInt8);
    getPin @2 (name: Text) -> (value: PinValue, error : UInt8);

    pinActions @3 (actions: List(PinAction)) -> (results: List(PinResult));
}

interface CMCtlThreads {

    struct Thread {
        name    @0  : Text;
        period  @1  : Int32;
        maxtime @2  : Int32;

    }

    getThreadList @0 () -> (threads: List(Thread));
} 

interface CMCtlSampler extends(CMCtlPins, CMCtlThreads) {

    struct SamplePoint {
        pinName @0: Text;
        value @1: CMCtlPins.PinValue;
        id @2: Float64;
    }

    struct SampleData {
        samples @0: List(SamplePoint);
    }

    startSampling @0 (pins: List(Text), depth: UInt32, threadName: Text, name: Text) -> (success: Bool); 
    stopSampling @1 (name: Text) -> (success: Bool);
    getSamplesData @2 (name: Text) -> (data: SampleData, success: Bool);
}

interface OpenCNServerInterface  extends(CMCtlPins) {

    struct CyclicData {
        feedoptStepDt       @0: Float64;
        feedoptQueueSize    @1: Int32;
        feedoptProgress     @2: Progress;
        homingFinished      @3: Bool;
        streamFinished      @4: Bool;
        streamRunning       @5: Bool;
        jogFinished         @6: Bool;
        gcodeFinished       @7: Bool;
        gcodeRunning        @8: Bool;
        currentPosition     @9: Position;
        spindleVelocity     @10: Float64;
        axisMode            @11: AxisMode;
        homed               @12: Bool;
        feedoptUsActive     @13: Bool;
        feedoptRtActive     @14: Bool;
        feedoptReady        @15: Bool;
        streamerFIFO        @16: Int32;
        machineMode         @17: MachineMode;
        feedoptQueueMax     @18: Int32;
        currentGCodeLine    @19: Int32;
        machineState        @20: UInt32;
        xLoad               @21: Float64;
        yLoad               @22: Float64;
        zLoad               @23: Float64;
        bLoad               @24: Float64;
        cLoad               @25: Float64;
        sLoad               @26: Float64;
        spindleTemp         @27: Float64;
    }

    struct FeedoptSample {
        x @0 :Float64;
        y @1 :Float64;
        z @2 :Float64;
    }

    struct Position {
        x @0 :Float64;
        y @1 :Float64;
        z @2 :Float64;
        b @3 :Float64;
        c @4 :Float64;
    }

    struct AxisMode {
        inactive    @0: AxisMask;
        fault       @1: AxisMask;
        homing      @2: AxisMask;
        csp         @3: AxisMask;
        csv         @4: AxisMask;
    }

    struct AxisMask {
        x @0 :Bool;
        y @1 :Bool;
        z @2 :Bool;
        b @3 :Bool;
        c @4 :Bool;
        spindle @5 :Bool;
    }

    struct MachineMode {
        homing      @0 :Bool;
        stream      @1 :Bool;
        jog         @2 :Bool;
        inactive    @3 :Bool;
        gcode       @4 :Bool;
    }

    struct Progress {
        compressingProgress @0 :Int32;
        compressingCount    @1 :Int32;
        smoothingProgress   @2 :Int32;
        smoothingCount      @3 :Int32;
        splittingProgress   @4 :Int32;
        splittingCount      @5 :Int32;
        optimisingProgress  @6 :Int32;
        optimisingCount     @7 :Int32;
        resamplingProgress  @8 :Int32;
        resamplingCount     @9 :Int32;
    }

    struct FeedOptCfg {
        nHorz      @0 :Int32;
        nDiscr     @1 :Int32;
        nBreak     @2 :Int32;
        lSplit     @3 :Float64;
        cutOff     @4 :Float64;
        debugPrint @5 :Bool;
        source @6 :Text;

        vmaxX @7  :Float64;
        vmaxY @8  :Float64;
        vmaxZ @9  :Float64;
        vmaxA @10 :Float64;
        vmaxB @11 :Float64;
        vmaxC @12 :Float64;

        amaxX @13 :Float64;
        amaxY @14 :Float64;
        amaxZ @15 :Float64;
        amaxA @16 :Float64;
        amaxB @17 :Float64;
        amaxC @18 :Float64;

        jmaxX @19 :Float64;
        jmaxY @20 :Float64;
        jmaxZ @21 :Float64;
        jmaxA @22 :Float64;
        jmaxB @23 :Float64;
        jmaxC @24 :Float64;
        vmax @25 :Float64;

        splitSpecialSpine @26 :Bool;
    }

    # == PINs related commands ==

    dummy0 @0 () -> ();
    dummy1 @1 () -> ();
    setFeedoptCommitCfg @2 (commit: Bool) -> ();
    getCyclicData @3 () -> (data: CyclicData);

    setLcctSetMachineModeHoming @4 (mode: Bool) -> ();
    setLcctSetMachineModeStream @5 (mode: Bool) -> ();
    setLcctSetMachineModeJog @6 (mode: Bool) -> ();
    setLcctSetMachineModeInactive @7 (mode: Bool) -> ();
    setLcctSetMachineModeGCode @8 (mode: Bool) -> ();

    dummy9 @9 () -> ();

    setStartHoming @10 (mode: Bool) -> ();
    setStopHoming  @11 (mode: Bool) -> ();

    setHomePositionX @12 (position: Float64) -> ();
    setHomePositionY @13 (position: Float64) -> ();
    setHomePositionZ @14 (position: Float64) -> ();

    setSpeedSpindle  @15 (speed: Float64) -> ();
    setActiveSpindle @16 (mode: Bool) -> ();
    setSpindleThreshold @52 (percent: Int32) -> ();

    setJogX @17 (mode: Bool) -> ();
    setJogY @18 (mode: Bool) -> ();
    setJogZ @19 (mode: Bool) -> ();

    setRelJog    @20  (value: Float64) -> ();
    setPlusJog   @21 (plus: Bool) -> ();
    setMinusJog @22 (minus: Bool) -> ();

    setAbsJog   @23 (value: Float64) -> ();
    setGoJog    @24 (mode: Bool)     -> ();
    setSpeedJog @25 (speed: Float64) -> ();
    setStopJog  @26 (mode: Bool)     -> ();

    setOffset  @27 (x: Float64, y: Float64, z: Float64, c: Float64) -> ();
    dummy28    @28 ();
    dummy29    @29 ();

    setStartStream @30 (mode: Bool) -> ();
    setStopStream  @31 (mode: Bool) -> ();

    setGcodeStart @32 (mode: Bool) -> ();
    setGcodePause @33 (mode: Bool) -> ();

    setFaultReset @34 (reset: Bool) -> ();

    setFeedrateScale   @35 (scale: Float64) -> ();
    setFeedoptReset    @36 (reset: Bool) -> ();

    # == Log related commands ==
    readLog @37 () -> (message: Text);

    # == share memory between feedopt and GUI related commands ==

    setFeedoptConfig @38 (config : FeedOptCfg) -> ();
    getFeedoptConfig @39 () -> (config : FeedOptCfg);

    # == Toolpath channel/sample support

    toolpathStartChannel @40 (sampleRate: Int32) -> (result : Bool);
    toolpathStopChannel @41 () -> ();

    struct PinValue {
        value :union {
            u @0: UInt32;
            s @1: Int32;
            f @2: Float64;
            b @3: Bool;
        }
    }

    struct Sample {
        values @0 :List(PinValue);
    }

    toolpathReadSamples  @42 () -> (samples: List(Sample));

    # == File support
    sendFileParam @43 (fileName: Text, size: UInt32, fileOp: Int32) -> (result: Int32);
    sendFileData @44 (data: Data) -> (result: Int32);
    pathExist @45 (path: Text) -> (result: Bool);
    createFolder @46 (folderPath: Text) -> (result: Bool);

    setLoadStream       @47 (mode: Bool) -> ();
    setPauseStream      @48 (mode: Bool) -> ();
    samplerNewFile @49 () -> ();
    getFileData @50 () -> (data: Data);
    setSamplerDownloadFile @51 (download: Bool) -> ();

    startJog @53 (axis: UInt8, target: Float64, relative: Bool) -> ();

    setFreeTool    @54 (state: Bool) -> ();
    setFreePalette @55 (state: Bool) -> ();

    struct Limit {
        min  @0 : Float64;
        max  @1 : Float64;
    }

    getMachineLimits @56 () -> (x: Limit, y : Limit, z : Limit,  B : Limit, c : Limit );

    # == Spindle ==

    getSpindleOverride @57 () -> (result : Float64);
    setSpindleOverride @58 (override : Float64) -> ();

    # == Lubrification ==

    getLubStatus @59 () -> (result : Bool);
    setLubForce @60 (lubForce : Bool) -> ();
    setLubWhenMilling @61 (lubWhenMilling : Bool) -> ();
}
