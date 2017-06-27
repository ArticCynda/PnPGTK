using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PnPConverter
{
  /// <summary>
  /// Represents a Pick and Place Machine.
  /// </summary>
  abstract public class PnPMachine
  {
    /// <summary>
    /// Creates a new instance of this Machine.
    /// </summary>
    public PnPMachine(){} 

    /// <summary>
    /// Returns a list of the Stacks available on this Machine.
    /// </summary>
    /// <param name="type">The type of stack.</param>
    /// <returns>A list of Stacks of the specified type.</returns>
    abstract public List<PnPStack> GetStacks(PnPStackType type);

    /// <summary>
    /// Returns the type of the Stack at a specified location.
    /// </summary>
    /// <param name="location">The location of the Stack.</param>
    /// <returns>The type of the Stack at the specified location.</returns>
    abstract public PnPStackType GetStackType(StackLocation location);

    /// <summary>
    /// Provides an overview of all the Stack types of which the Machine supports at least one.
    /// </summary>
    /// <returns>A list of Stack types supported by this Machine.</returns>
    abstract public PnPStackType[] GetStackTypes();

    /// <summary>
    /// Provides an overview of packages that can be loaded into a Stack of specified type on this Machine. If the Stack type is Undefined, all packages supported by this Machine will be returned.
    /// </summary>
    /// <returns>A list of packages supported by the specified Stack type on this Machine.</returns>
    abstract public PnPSupplyPackage[] GetSupportedPackages(PnPStackType stackType);

    /// <summary>
    /// Retrieves the Stack at the specified location on this Machine.
    /// </summary>
    /// <param name="location">The location of the Stack.</param>
    /// <returns>The Stack at the specified location.</returns>
    abstract public PnPStack GetStack(StackLocation location);

    /// <summary>
    /// Exports the Stack and Part information to a set of Pick and Place compatible files at the specified location in the file system.
    /// </summary>
    /// <param name="directory">The location in the file system where the files should be placed. Existing files will be overwritten.</param>
    /// <param name="fileNamePrefix">The prefix of the file names, commonly the PCB or project name. An index with the respective phase will automatically be appended.</param>
    /// <param name="stacks">The Stack configuration for this Machine.</param>
    /// <param name="parts">A list of Parts to be placed on the PCB.</param>
    abstract public void ExportPnPData(DirectoryInfo directory, string fileNamePrefix, List<PnPStack> stacks, List<PnPPart> parts, PnPPanel panel);
  }

  /// <summary>
  /// Represents the size of a Nozzle for a Pick and Place Machine.
  /// </summary>
  public enum PnPMachineNozzle
  {
    Unsupported,
    Undefined,
    XS,
    S,
    M,
    L
  }

  /// <summary>
  /// The exception that is thrown when a nozzle is selected that is not supported by the machine.
  /// </summary>
  public class NozzleNotSupportedException: ArgumentException
  {
    public override string Message
    {
      get
      {
        return "The specified nozzle is not supported by this machine.";
      }
    }
  }
}
