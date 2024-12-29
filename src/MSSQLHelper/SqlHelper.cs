﻿using Foundation.MSSQLHelper.Models;
using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Foundation.MSSQLHelper.MSSQLHelper;
public class SqlHelper: ISqlHelper
{

    public SqlHelper()
    {

    }
    private SqlConnection CreateConnection(MSSQLConnectionModel connection)
    {
        var connectionObject = new SqlConnection
        {
            ConnectionString = new SqlConnectionStringBuilder
            {
                DataSource = connection.ServerName,
                InitialCatalog = connection.DataBaseName,
                UserID = connection.UserId,
                Password = connection.Password,
                IntegratedSecurity = false,
                TrustServerCertificate = true,
                ConnectTimeout = connection.ConnectionTimeout,
                ConnectRetryCount = connection.ConnectionRetryCount,
                ConnectRetryInterval = connection.ConnectionRetryInterval,
                MaxPoolSize = 200

            }.ConnectionString
        };

        return connectionObject;
    }

    private void CloseConnection(DbConnection connectionObject)
    {
        if (connectionObject != null &&
            (connectionObject.State != ConnectionState.Broken ||
            connectionObject.State != ConnectionState.Closed))

            connectionObject.Close();
    }

    private async Task OpenConnectionAsync(SqlConnection connectionObject)
    {
        await connectionObject.OpenAsync();
    }

    public async Task<dynamic?> FetchData(ExecuteDataSetRequest? request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(Constants.RequestIsNull);
        }

        if (request.ConnectionProperties == null)
        {
            throw new ArgumentNullException(Constants.DataBaseNameIsNull);
        }

        if (string.IsNullOrWhiteSpace(request.CommandText))
        {
            throw new ArgumentNullException(Constants.CommandTextIsNull);
        }

        if (request.IsMultipleTables)
        {
            return await FetchDataSet(request.ConnectionProperties, request.CommandText, request.CommandType, request.CommandTimeout, request.Parameters);
        }
        else
        {
            return await FetchDataTable(request.ConnectionProperties, request.CommandText, request.CommandType, request.CommandTimeout, request.Parameters);
        }
    }

    public async Task<DataTable> FetchDataTable(MSSQLConnectionModel connectionParams, string commandText, CommandType commandType, int commandTimeout, params IDbDataParameter []? parameters)
    {
        var dt = new DataTable();
#pragma warning disable CA1062 // Validate arguments of public methods
        using SqlConnection connectionObject = CreateConnection(connectionParams);
#pragma warning restore CA1062 // Validate arguments of public methods
        try
        {
            using var commandObject = new SqlCommand(commandText, connectionObject);
            commandObject.CommandType = commandType;

            if (parameters != null && parameters.Length > 0)
            {
                commandObject.Parameters.AddRange(parameters);
            }

            commandObject.CommandTimeout = commandTimeout;

            using var adapterObject = new SqlDataAdapter(commandObject);

            await OpenConnectionAsync(connectionObject);

            await Task.Run(() =>
            {
                adapterObject.Fill(dt);
            });

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            CloseConnection(connectionObject);
        }

        return dt;
    }

    public async Task<DataSet> FetchDataSet(MSSQLConnectionModel connectionParams, string commandText, CommandType commandType, int commandTimeout, params IDbDataParameter []? parameters)
    {
        var resultSet = new DataSet();
#pragma warning disable CA1062 // Validate arguments of public methods
        using SqlConnection connectionObject = CreateConnection(connectionParams);
#pragma warning restore CA1062 // Validate arguments of public methods
        try
        {
            using var commandObject = new SqlCommand(commandText, connectionObject);
            commandObject.CommandType = commandType;

            if (parameters != null && parameters.Length > 0)
            {
                commandObject.Parameters.AddRange(parameters);
            }

            commandObject.CommandTimeout = commandTimeout;

            using var adapterObject = new SqlDataAdapter(commandObject);

            await OpenConnectionAsync(connectionObject);

            await Task.Run(() =>
            {
                adapterObject.Fill(resultSet);
            });

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            CloseConnection(connectionObject);
        }

        return resultSet;
    }

    public async Task<int> ExecuteNonQuery(ExecuteNonQueryRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(Constants.RequestIsNull);
        }

        if (request.ConnectionProperties == null)
        {
            throw new ArgumentNullException(Constants.ConnectionPropertiesIsNull);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.ConnectionProperties.ServerName))
            {
                throw new ArgumentNullException(Constants.DBServerNameIsNull);
            }

            if (string.IsNullOrWhiteSpace(request.ConnectionProperties.DataBaseName))
            {
                throw new ArgumentNullException(Constants.DBNameIsNull);
            }

            if (string.IsNullOrWhiteSpace(request.ConnectionProperties.UserId))
            {
                throw new ArgumentNullException(Constants.DBUserIdIsNull);
            }

            if (string.IsNullOrWhiteSpace(request.ConnectionProperties.Password))
            {
                throw new ArgumentNullException(Constants.DBPasswordIsNull);
            }
        }

        if (string.IsNullOrWhiteSpace(request.CommandText))
        {
            throw new ArgumentNullException(Constants.CommandTextIsNull);
        }

        return await ExecuteNonQuery(request.ConnectionProperties, request.CommandText, request.CommandType, request.CommandTimeout, request.Parameters);
    }

    public async Task<int> ExecuteNonQuery(MSSQLConnectionModel connectionParams, string commandText, CommandType commandType, int commandTimeout, params IDbDataParameter []? parameters)
    {
        var numRowsAffected = 0;
#pragma warning disable CA1062 // Validate arguments of public methods
        using SqlConnection connectionObject = CreateConnection(connectionParams);
#pragma warning restore CA1062 // Validate arguments of public methods
        try
        {
            using var commandObject = new SqlCommand(commandText, connectionObject);
            commandObject.CommandType = commandType;
            commandObject.CommandTimeout = commandTimeout;
            if (parameters != null && parameters.Length > 0)
            {
                commandObject.Parameters.AddRange(parameters);
            }

            await OpenConnectionAsync(connectionObject);

            numRowsAffected = await commandObject.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            CloseConnection(connectionObject);
        }

        return numRowsAffected;
    }
}
