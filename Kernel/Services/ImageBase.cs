using System.Drawing;
using Kernel.Domain.Interfaces;
using Kernel.Domain.Settings;
using Kernel.Domain.Utils;

namespace Kernel.Services;

public class ImageBase : Configurer<ImageSettings>
{
    private readonly List<(IRenderable renderable, Func<Color, Color, Color> action)> items;

    private ImageBase()
    {
        items = new List<(IRenderable, Func<Color, Color, Color>)>();
    }

    public static ConfigurationContext<ImageBase, ImageBase, ImageSettings> Create()
    {
        var imageBase = new ImageBase();
        return new ConfigurationContext<ImageBase, ImageBase, ImageSettings>(
            imageBase,
            imageBase
        );
    }

    public ImageBase Add<TRenderable>(Func<TRenderable, TRenderable> renderable)
        where TRenderable : IRenderable
    {
        var obj = typeof(TRenderable)
            .GetConstructor(new[] { typeof(int), typeof(int) })?.Invoke(new[]
            {
                Settings.Width, (object)Settings.Height
            });
        var renderable1 = renderable.DynamicInvoke(obj) as IRenderable;
        items.Add(
            (renderable1,
                (x, y) => x.Add(y))
        );

        return this;
    }


    public ImageBase Multiply<TRenderable>(Func<TRenderable, TRenderable> renderableFactory)
        where TRenderable : IRenderable
    {
        var obj = typeof(TRenderable)
            .GetConstructor(new[] { typeof(int), typeof(int) })?.Invoke(new[]
            {
                Settings.Width, (object)Settings.Height
            });
        var renderable = renderableFactory.DynamicInvoke(obj) as IRenderable;
        items.Add(
            (renderable,
                (x, y) => x.Multiply(y))
        );
        return this;
    }

    public DirectBitmap GetBitmap(DirectBitmap baseBitmap)
    {
        foreach (var (renderable, action) in items)
        {
            baseBitmap = renderable.GetBitmap(baseBitmap, action);
        }

        return baseBitmap;
    }
    public DirectBitmap GetBitmap(DirectBitmap baseBitmap, int i)
    {
        foreach (var (renderable, action) in items)
        {
            baseBitmap = renderable.GetBitmap(baseBitmap, action, i);
        }

        return baseBitmap;
    }
}

public abstract class Configurer<TSettings> : IConfigurer<TSettings>
{
    public TSettings Settings { get; protected set; }

    void IConfigurer<TSettings>.Set(TSettings settings)
    {
        Settings = settings;
    }
}

public class ConfigurationContext<TMainContext, TConfigureContext, TConfiguration>
    where TConfigureContext : IConfigurer<TConfiguration>
{
    private readonly TMainContext mainContext;
    private readonly TConfigureContext configureContext;

    public ConfigurationContext(TMainContext mainContext, TConfigureContext configureContext)
    {
        this.mainContext = mainContext;
        this.configureContext = configureContext;
    }

    public TMainContext Config(TConfiguration configuration)
    {
        configureContext.Set(configuration);
        return mainContext;
    }
}

public interface IConfigurer<in TConfiguration>
{
    void Set(TConfiguration configuration);
}