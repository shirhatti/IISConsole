using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace IISConsole
{
    internal static class IpmTypes
    {
        internal enum IPM_OPCODE
        {
            IPM_OP_MINIMUM = 0,
            IPM_OP_PING,
            IPM_OP_PING_REPLY,
            IPM_OP_WORKER_REQUESTS_SHUTDOWN,
            IPM_OP_SHUTDOWN,
            IPM_OP_REQUEST_COUNTERS,
            IPM_OP_SEND_COUNTERS,
            IPM_OP_PERIODIC_PROCESS_RESTART_PERIOD_IN_MINUTES,
            IPM_OP_PERIODIC_PROCESS_RESTART_MEMORY_USAGE_IN_BYTES,
            IPM_OP_PERIODIC_PROCESS_RESTART_SCHEDULE,
            IPM_OP_GETPID,
            IPM_OP_HRESULT,
            IPM_OP_START_QUEUE,
            IPM_OP_STOP_QUEUE,
            IPM_OP_REPORT_QUEUE_STARTED,
            IPM_OP_REPORT_QUEUE_STOPPED,
            IPM_OP_NOTIFY_OF_PENDING_SHUTDOWN,
            IPM_OP_REQUEST_CUSTOM_ACTION,
            IPM_OP_REPORT_CUSTOM_ACTION_RESULT,
            IPM_OP_CENTRAL_LOGGING_ENABLED,
            IPM_OP_ANONYMOUS_TOKEN_HANDLE,
            IPM_OP_IDLE_TIME,
            IPM_OP_JOB_UPDATE,
            IPM_OP_PRELOAD_APP,
            IPM_OP_PRELOAD_COMPLETE,
            IPM_OP_THREAD_AFFINITY_POLICY,

            // Protocol Communications
            IPM_OP_PROT_REGISTER,
            IPM_OP_PROT_DEREGISTER,
            IPM_OP_PROT_START_QUEUE,
            IPM_OP_PROT_STOP_QUEUE,
            IPM_OP_PROT_CREATE_POOL,
            IPM_OP_PROT_DELETE_POOL,
            IPM_OP_PROT_CREATE_APP,
            IPM_OP_PROT_DELETE_APP,
            IPM_OP_PROT_ENABLE_POOL,
            IPM_OP_PROT_DISABLE_POOL,
            IPM_OP_PROT_CALL_ON_NEXT_REQUEST,
            IPM_OP_PROT_SERVER_CONNECTED,
            IPM_OP_PROT_REQUEST_OK_TO_TERMINATE,
            IPM_OP_PROT_OK_TO_TERMINATE,
            IPM_OP_PROT_UPDATE_POOL_ID,
            IPM_OP_PROT_SET_PROTOCOL_ACTIVE_STATE,
            IPM_OP_PROT_ALL_QUEUE_INSTANCES_STOPPED,
            IPM_OP_PROT_UPDATE_APP_APP_POOL,
            IPM_OP_PROT_NEW_APP_BINDINGS,
            IPM_OP_PROT_UPDATE_REQUESTS_BLOCKED,
            IPM_OP_PROT_POOL_ID_UPDATE_ACK,
            IPM_OP_PROT_MAX_BLOB_SIZE,

            IPM_OP_MAXIMUM
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct IPM_OPCODE_METADATA
        {
            readonly int sizeMaximumAndRequiredSize;
            readonly bool fServerSideExpectsThisMessage;
        };

        [StructLayout(LayoutKind.Sequential)]
        internal struct IPM_MESSAGE_HEADER
        {
            public IPM_MESSAGE_HEADER(IPM_OPCODE opCode, uint messageSize)
            {
                OpCode = opCode;
                MessageSize = messageSize;
            }
            internal IPM_OPCODE OpCode { get; }
            internal uint MessageSize { get; set; }

        };


        internal static readonly IPM_OPCODE_METADATA[] HttpVerbs = new IPM_OPCODE_METADATA[]
        {
            new IPM_OPCODE_METADATA() { },
            new IPM_OPCODE_METADATA()
        };

        internal enum IPM_WP_SHUTDOWN_MSG
        {
            IPM_WP_MINIMUM = 0,

            IPM_WP_RESTART_COUNT_REACHED,
            IPM_WP_IDLE_TIME_REACHED,
            IPM_WP_RESTART_SCHEDULED_TIME_REACHED,
            IPM_WP_RESTART_ELAPSED_TIME_REACHED,
            IPM_WP_RESTART_VIRTUAL_MEMORY_LIMIT_REACHED,
            IPM_WP_RESTART_ISAPI_REQUESTED_RECYCLE,
            IPM_WP_RESTART_PRIVATE_BYTES_LIMIT_REACHED,
            IPM_WP_RESTART_PRELOAD_FAILURE,
            IPM_WP_RESTART_CONFIG_READ_ERROR,

            IPM_WP_MAXIMUM
        };
    }
}
